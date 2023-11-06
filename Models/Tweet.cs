using System.Text.Json.Serialization;

namespace Twitter.Models;

public record TweetItem
{
    [JsonPropertyName("tweet_id")]
    public int TweetId { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

      [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    

    
}