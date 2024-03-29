using System.Text.Json.Serialization;

namespace Twitter.Models;

public record CommentItem
{
    [JsonPropertyName("comment_id")]
    public int CommentId { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("tweet_id")]
    public int TweetId { get; set; }

    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonPropertyName("user_name")]
    public string UserName { get; set; }
    
}