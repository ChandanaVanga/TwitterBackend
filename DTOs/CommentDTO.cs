using System.ComponentModel.DataAnnotations;

namespace Twitter.DTOs;

public record CommentCreateDTO
{
    [Required]
    [MinLength(3)]
    [MaxLength(60)]
    public string Text { get; set; }

    [Required]
    public int TweetId { get; set; }

    [Required]
    public int CommentId { get; set; }
}

public record CommentUpdateDTO
{
    [MinLength(3)]
    [MaxLength(60)]
    public string Text { get; set; } = null;

    public bool? UpdatedAt { get; set; } = null;
}
