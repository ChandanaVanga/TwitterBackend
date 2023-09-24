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
[Route("api/comment")]
public class CommentController : ControllerBase
{
    private readonly ILogger<CommentController> _logger;
    private readonly ICommentRepository _comment;

    public CommentController(ILogger<CommentController> logger,
    ICommentRepository comment)
    {
        _logger = logger;
        _comment = comment;
    }

    private int GetUserIdFromClaims(IEnumerable<Claim> claims)
    {
        return Convert.ToInt32(claims.Where(x => x.Type == TwitterConstants.UserId).First().Value);
    }

    [HttpPost]
    public async Task<ActionResult<CommentItem>> CreateComment([FromBody] CommentCreateDTO Data)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var toCreateItem = new CommentItem
        {
            Text = Data.Text.Trim(),
            UserId = userId,
            TweetId = Data.TweetId,
            CommentId = Data.CommentId
        };

        // Insert into DB
        var createdItem = await _comment.Create(toCreateItem);

        // Return the created Todo
        return StatusCode(201, createdItem);
    }

    

    [HttpDelete("{comment_id}")]
    public async Task<ActionResult> DeleteComment([FromRoute] int comment_id)
    {
        var userId = GetUserIdFromClaims(User.Claims);

        var existingItem = await _comment.GetById(comment_id);

        if (existingItem is null)
            return NotFound();

        if (existingItem.UserId != userId)
            return StatusCode(403, "You cannot delete other's Comments");

        await _comment.Delete(comment_id);

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<List<CommentItem>>> GetAllComments()
    {
        var allComments = await _comment.GetAll();
        return Ok(allComments);
    }

    [HttpGet("{tweet_id}")]
    public async Task<ActionResult<List<CommentItem>>> GetAllCommentsByTweetId([FromRoute] int tweet_id)
    {
        var allComment = await _comment.GetCommentsByTweetId(tweet_id);
        return Ok(allComment);
    }
}
