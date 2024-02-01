using Reddit.Controllers;
using RedditReader.Services;

namespace RedditReader
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IRedditPostService _redditPostService;

        public Worker(ILogger<Worker> logger, IRedditPostService redditPostService)
        {
            _logger = logger;
            _redditPostService = redditPostService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _redditPostService.MonitorRedditStats();

            while (!stoppingToken.IsCancellationRequested)
            {
                //if (_logger.IsEnabled(LogLevel.Information))
                //{
                //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //}

                //var newPosts = _redditPostService.MonitorRedditStats();
                //_posts.AddRange(newPosts);

                //Console.WriteLine($"");


                //lastId = newPosts.Count > 0 ? newPosts.First().Id : lastId;

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
