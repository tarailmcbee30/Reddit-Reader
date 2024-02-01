using Microsoft.Extensions.Options;
using Reddit;
using Reddit.Controllers;
using Reddit.Controllers.EventArgs;
using Reddit.Things;
using RedditReader.Models;

namespace RedditReader.Services
{
    public class RedditPostService : IRedditPostService
    {
        private readonly IRedditAuthService _authService;
        private readonly RedditVariables _redditVariables;
        private readonly RedditClient _redditClient;

        List<Reddit.Controllers.Post> _posts = new List<Reddit.Controllers.Post>();

        public RedditPostService(IRedditAuthService redditAuthService, IOptions<RedditVariables> options)
        {
            _authService = redditAuthService;
            _redditVariables = options.Value;

            var accessToken = _authService.GetAccessToken();
            _redditClient = new RedditClient(_redditVariables.AppId, appSecret: _redditVariables.AppSecret, accessToken: accessToken);
        }

        public void MonitorRedditStats()
        {
            // Start monitoring the subreddit for new comments and register the callback function.  --Kris
            var subreddit = _redditClient.Subreddit(_redditVariables.SubredditName);

            //subreddit.Posts.GetNew();  // This call prevents any existing "new"-sorted comments from triggering the update event.  --Kris
            subreddit.Posts.MonitorNew();

            subreddit.Posts.NewUpdated += PostsUpdated;
        }

        // Posts with most up votes
        // Users with most posts

        private void PostsUpdated(object sender, PostsUpdateEventArgs e)
        {
            _posts.AddRange(e.Added);

            var postWithUpvote = _posts.MaxBy(p => p.UpVotes);
            var usersAndCount = _posts.GroupBy(p => p.Author).Select(x => new { Author = x.Key, PostsCount = x.Count() });
            var userWithMostPosts = usersAndCount.MaxBy(x => x.PostsCount);

            Console.WriteLine($"Result set on time: {DateTime.UtcNow}");
            Console.WriteLine($"Posts with most Upvotes is: ({postWithUpvote.Title}) with Upvotes: {postWithUpvote.UpVotes}");
            Console.WriteLine($"User with Most Posts is: {userWithMostPosts.Author} with Post Count: {userWithMostPosts.PostsCount}");
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
