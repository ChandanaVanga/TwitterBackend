using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Twitter.DTOs;

public record UsersLoginDTO
{
    // [Required]
    // [JsonPropertyName("username")]
    // [MinLength(3)]
    // [MaxLength(255)]
    // public string UserName { get; set; } // not needed. Remove required attribute

    [Required]
    [JsonPropertyName("email")] // small e
    [MinLength(3)]
    [MaxLength(255)]
    public string Email { get; set; }

    [Required]
    [JsonPropertyName("passwor_d")]
    [MaxLength(255)]
    public string Password { get; set; }
}

public record UsersLoginResDTO
{
    [JsonPropertyName("token")]
    public string Token { get; set; }

    // [JsonPropertyName("username")]
    // public string UserName { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("email")]

    public string Email { get; set; }

    [JsonPropertyName("user_name")]
    public string UserName { get; set; }
}

public record UsersCreateDTO
{
    // [Required]
    // [JsonPropertyName("user_id")]
    // [MaxLength(255)]
    // public string UserId { get; set; }

    [Required]
    [JsonPropertyName("email")]
    [MinLength(3)]
    [MaxLength(255)]
    public string Email { get; set; }
    [Required]
    [JsonPropertyName("name")]
    [MinLength(3)]
    [MaxLength(255)]
    public string UserName { get; set; }

    [Required]
    [JsonPropertyName("passwor_d")]
    [MaxLength(255)]
    public string Password { get; set; }
}


public record UsersCreateResDTO
{


    [Required]
    [JsonPropertyName("email")]
    [MinLength(3)]
    [MaxLength(255)]
    public string Email { get; set; }
    [Required]
    [JsonPropertyName("name")]
    [MinLength(3)]
    [MaxLength(255)]
    public string UserName { get; set; }

    // [Required]
    // [JsonPropertyName("user_id")]
    // [MaxLength(255)]
    // public int UserId { get; set; }
}

public record UsersLoginUpdateDTO
{



    [Required]
    [JsonPropertyName("name")]
    [MinLength(3)]
    [MaxLength(255)]
    public string UserName { get; set; }


    // [JsonPropertyName("email")]

    // public string Email { get; set; }


}
