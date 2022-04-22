using Twitter.Models;
using Twitter.Repositories;
using Microsoft.AspNetCore.Mvc;
using Twitter.DTOs;
using Microsoft.AspNetCore.Authorization;
using Twitter.Utilities;
using System.Security.Claims;

namespace Twitter.Controllers;

[ApiController]
[Authorize]
[Route("api/tweet")]
public class TweetController : ControllerBase
{
    private readonly ILogger<TweetController> _logger;
    private readonly ITweetRepository _tweet;  

    private readonly ICommentRepository _comment;  

    public TweetController(ILogger<TweetController> logger,
    ITweetRepository tweet, ICommentRepository comment)
    {
        _logger = logger;
        _tweet = tweet;
        _comment = comment;
    }

    private int GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == TwitterConstants.UserId).First().Value);
    }

    [HttpPost]
    public async Task<ActionResult<TweetItem>> CreateTweet([FromBody] TweetCreateDTO Data)
    {
        var userId = GetUserIdFromClaims(User.Claims);

         List<TweetItem> usertweets = await _tweet.GetTweetsByUserId(userId);
        if (usertweets != null && usertweets.Count >= 5)
        {
            return BadRequest("Limit exceeded");
        }

        var toCreateItem = new TweetItem
        {
            Title = Data.Title.Trim(),
            UserId = userId,

        };

        // var toCreatedItem = new TweetItem
        // {
        //     Title = Data.Title.Trim(),
        //     //UserId = userId,
        // };

        // Insert into DB
        var createdItem = await _tweet.Create(toCreateItem);

        // Return the created Todo
        return StatusCode(201, createdItem);
    }

    [HttpPut("{tweet_id}")]
    public async Task<ActionResult> UpdateTweet([FromRoute] int tweet_id,
    [FromBody] TweetUpdateDTO Data)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var existingItem = await _tweet.GetById(tweet_id);

        if (existingItem is null)
            return NotFound();

        if (existingItem.UserId != userId)
            return StatusCode(403, "You cannot update other's Tweets");

        var toUpdateItem = existingItem with
        {
            Title = Data.Title is null ? existingItem.Title : Data.Title.Trim(),
            //UpdatedAt = Data.UpdatedAt,
        };

        await _tweet.Update(toUpdateItem);

        return NoContent();
    }

    [HttpDelete("{tweet_id}")]
    public async Task<ActionResult> DeleteTweet([FromRoute] int tweet_id)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var existingItem = await _tweet.GetById(tweet_id);

        if (existingItem is null)
            return NotFound();

        if (existingItem.UserId != userId)
            return StatusCode(403, "You cannot delete other's Tweets");

        await _comment.DeleteByTweetId(tweet_id);    

        await _tweet.Delete(tweet_id);

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<TweetItem>>> GetAllTweets()
    {
        var allTweets = await _tweet.GetAll();
        return Ok(allTweets);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TweetItem>> GetTweetById([FromRoute] int id)
    {
        var singleTweet = await _tweet.GetById(id);
        return Ok(singleTweet);
    }
}
