using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using MyEntity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyClient
{
	class Program
    {
        private static HubConnection connection;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Processing started... Press any key to stop execution.");

            connection = new HubConnectionBuilder()
              .WithUrl("http://localhost:5000/streamHub")
              .ConfigureLogging(logging =>
              {
                  logging.AddDebug();
                  logging.SetMinimumLevel(LogLevel.Debug);
              })
              .Build();

            var cancellationTokenSource = new CancellationTokenSource();
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
			{
                await connection.StartAsync();

                var stream = connection.StreamAsync<TwitterStats>("SendTwitterStats", cancellationTokenSource.Token);

                await foreach (var twitterStats in stream)
                {
                    if (Console.KeyAvailable)
                    {
                        var consoleKey = Console.ReadKey(true);  
                        Console.CursorTop = 0;
                        Console.CursorLeft = 0;
                        Console.WriteLine("Streaming stopped.");
                        await connection.StopAsync(cancellationTokenSource.Token);
                    }
                    Console.CursorTop = 1;
                    Console.CursorLeft = 0;
                    Console.Write($"Count tweets total: {twitterStats.Count}");
                    Console.CursorTop = 2;
                    Console.CursorLeft = 0;
                    Console.Write($"Average tweets per minute: {twitterStats.Average}");
                    Console.CursorTop = 3;
                    Console.CursorLeft = 0;
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.CursorTop = 4;
                    Console.CursorLeft = 0;
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.CursorTop = 3;
                    Console.CursorLeft = 0;
                    Console.Write($"hashtag: {twitterStats.TrendHashtags}");
                }
            }
            catch (System.OperationCanceledException)
			{
                Console.CursorTop = 5;
                Console.CursorLeft = 0;
                Console.Write(new string(' ', Console.WindowWidth));
                Console.WriteLine("Processing ending... Cancellation was requested.");
            }
            finally
			{
                cancellationTokenSource.Token.ThrowIfCancellationRequested();
                await connection.DisposeAsync();
            }

            Console.CursorTop = 6;
            Console.CursorLeft = 0;
            Console.Write(new string(' ', Console.WindowWidth));
            Console.WriteLine("Streaming completed");
            Console.ReadLine();
        }
    }
}
