namespace Twitter.Utilities;

public static class TwitterConstants
{
    public const string UserId = nameof(UserId);
    public const string Email = nameof(Email); // "Username"
}

public enum TableNames
{
    users,
    tweet,

    comment,
}