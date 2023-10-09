using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Twitter.DTOs;

public record TweetCreateDTO
{
    [Required]
    [MinLength(3)]
    [MaxLength(90)]
    public string Title { get; set; }

    // [Required]
    // public int TweetId { get; set; }

    // [JsonPropertyName("user_id")]
    // [MaxLength(255)]
    // public int UserId { get; set; }

    // [JsonPropertyName("created_at")]
    // [MaxLength(255)]
    // public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    // [JsonPropertyName("updated_at")]
    // [MaxLength(255)]
    // public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    // [Required]
    // public int UserId { get; set; }
}

public record TweetUpdateDTO
{
    [MinLength(3)]
    [MaxLength(90)]
    public string Title { get; set; } = null;

    public DateTimeOffset UpdatedAt { get; set; } 
}
