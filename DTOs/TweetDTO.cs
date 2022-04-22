using System.ComponentModel.DataAnnotations;

namespace Twitter.DTOs;

public record TweetCreateDTO
{
    [Required]
    [MinLength(3)]
    [MaxLength(90)]
    public string Title { get; set; }

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
