using RedditReader.Models;
using RedditReader.Services;

namespace RedditReader
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddOptions<RedditVariables>().BindConfiguration(nameof(RedditVariables));
            builder.Services.AddOptions<SystemVariables>().BindConfiguration(nameof(SystemVariables));

            builder.Services.AddTransient<IRedditAuthService, RedditAuthService>();
            builder.Services.AddTransient<IRedditPostService, RedditPostService>();

            builder.Services.AddHostedService<Worker>();

            var host = builder.Build();
            host.Run();
        }
    }
}