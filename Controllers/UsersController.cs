using Twitter.Models;
using Twitter.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Twitter.Utilities;
using Twitter.DTOs;

namespace Twitter.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUsersRepository _users;
    private readonly IConfiguration _config;

    public UsersController(ILogger<UsersController> logger,
    IUsersRepository users, IConfiguration config)
    {
        _logger = logger;
        _users = users;
        _config = config;
    }

    private int GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == TwitterConstants.UserId).First().Value);
    }

     [HttpPost("login")]
    public async Task<ActionResult<UsersLoginResDTO>> Login(
        [FromBody] UsersLoginDTO Data
    )
    {
        var existingUser = await _users.GetByEmail(Data.Email);

        if (existingUser is null)
            return NotFound("User with given email id not found");
        bool verified = BCrypt.Net.BCrypt.Verify(Data.Password, existingUser.Password);

        if (!verified)
            return BadRequest("Incorrect password");

        var token = Generate(existingUser);

        var res = new UsersLoginResDTO
        {
            UserId = existingUser.UserId,
            Email = existingUser.Email,
            Token = token,
            UserName = existingUser.UserName,
        };

        return Ok(res);
    }



    private string Generate(Users users)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(TwitterConstants.UserId, users.UserId.ToString()),
            new Claim(TwitterConstants.Email, users.Email),
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    [HttpPost("registration")]
    public async Task<ActionResult<UsersCreateResDTO>> Registration(
        [FromBody] UsersCreateDTO Data
    )
    {
        var existingUser = await _users.GetByEmail(Data.Email);

        if (existingUser != null)
            return BadRequest("User with given email already exist");

        Users newUser = new Users
        {
            UserName = Data.UserName,
            Email = Data.Email?.Trim(),
            Password = BCrypt.Net.BCrypt.HashPassword(Data.Password),
        };

        var createdUser = await _users.CreateNewUser(newUser);



        var res = new UsersCreateResDTO
        {
            // UserId = createdUser.UserId,
            Email = createdUser.Email,
            UserName = createdUser.UserName,


        };

        return Ok(res);
    }


    [HttpPut("{user_id}")]
    public async Task<ActionResult> UpdateUsers([FromRoute] int user_id,
    [FromBody] UsersLoginUpdateDTO Data)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var existingItem = await _users.GetById(user_id);

        if (existingItem is null)
            return NotFound();

        if (existingItem.UserId != userId)
            return StatusCode(403, "You cannot update other's UserName");

        var toUpdateItem = existingItem with
        {
            UserName = Data.UserName is null ? existingItem.UserName : Data.UserName.Trim(),
            //Email = Data.Email,
        };

        await _users.Update(toUpdateItem);

        return NoContent();
    }
}
