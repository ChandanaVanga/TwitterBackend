using Twitter.Models;
using Dapper;
using Twitter.Utilities;

namespace Twitter.Repositories;

public interface ICommentRepository
{
    Task<CommentItem> Create(CommentItem Item);
    Task Delete(int CommentId);
    Task<List<CommentItem>> GetAll();

    Task<CommentItem> GetById(int CommentId);

    Task<List<CommentItem>> GetCommentsByTweetId(int TweetId);
    Task DeleteByTweetId(int TweetId);
    
}

public class CommentRepository : BaseRepository, ICommentRepository
{
    public CommentRepository(IConfiguration config) : base(config)
    {

    }

  

    public async Task Delete(int CommentId)
    {
        var query = $@"DELETE FROM {TableNames.comment} WHERE comment_id = @CommentId";

        using (var con = NewConnection)
            await con.ExecuteAsync(query, new { CommentId });
    }


    public async Task<List<CommentItem>> GetAll()
    {
        // var query = $@"SELECT * FROM {TableNames.comment} ORDER BY created_at DESC";

        var query = $@"SELECT c.*, u.user_name
              FROM {TableNames.comment} AS c
              JOIN {TableNames.users} AS u ON c.user_id = u.user_id
              ORDER BY c.created_at DESC";

        using (var con = NewConnection)
            return (await con.QueryAsync<CommentItem>(query)).AsList();
    }

    public async Task<CommentItem> Create(CommentItem Item)
    {
        var query = $@"INSERT INTO {TableNames.comment} (text, tweet_id) 
        VALUES (@Text, @TweetId) RETURNING *";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<CommentItem>(query, Item);
    }

    public async Task<CommentItem> GetById(int CommentId)
    {
        var query = $@"SELECT * FROM {TableNames.comment} WHERE comment_id = @CommentId";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<CommentItem>(query, new { CommentId });
    }

    public async Task<List<CommentItem>> GetCommentsByTweetId(int TweetId)
    {
        //  var query = $@"SELECT * FROM {TableNames.comment} WHERE tweet_id = @TweetId";

        var query = $@"SELECT c.*, u.user_name
              FROM {TableNames.comment} AS c
              JOIN {TableNames.users} AS u ON c.user_id = u.user_id
              WHERE c.tweet_id = @TweetId";


        using (var con = NewConnection)
        return (await con.QueryAsync<CommentItem>(query, new { TweetId })).AsList();
    }

    public async Task DeleteByTweetId(int TweetId)
    {
        var query = $@"DELETE FROM {TableNames.comment} WHERE tweet_id = @TweetId";

        using (var con = NewConnection)
            await con.ExecuteAsync(query, new { TweetId });
    }
}