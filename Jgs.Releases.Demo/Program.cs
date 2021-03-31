using JgsReleases;
using JgsReleases.Dto;
using JgsReleases.Infrastructure.Progress;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Jgs.Releases.Demo
{
    class Program
    {
        public static void Output(string message)
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}]\t{message}");
        }

        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            Task.Run(() => Run(token));
            Output("Started");
            while (Console.ReadKey().Key != ConsoleKey.C)
            {
                Thread.Sleep(500);
                Output("STEP");
            }
            Console.WriteLine("");

            Output("Call cancel");
            cts.Cancel();
            cts.Token.WaitHandle.WaitOne();
            Output("DONE");
            Console.ReadLine();
        }

        private static void OnUploadChanged(FileProgressArgs args)
        {
            Console.WriteLine(args);
        }

        private static async Task Run(CancellationToken? cancellationToken = null)
        {
            var token = cancellationToken ?? CancellationToken.None;
            var key = System.IO.File.ReadAllText("Key.json");
            var client = new ReleasesClient(key);
            await client.Login();
            var list = await client.GetReleasesAsync();
            var latest = list.FirstOrDefault(f => f.IsLatest);
            if (latest != null)
            {
                var dResult = await client.Download
                    .Assets(latest.Assets)
                    .WithProgress(OnUploadChanged)
                    .WithCancellationToken(token)
                    .Start();
                Console.WriteLine(dResult);
                Console.WriteLine("Done");
            }
            return;
            var newRelease = new NewReleaseArguments
            {
                TagName = "v.0.0.2",
                Draft = true,
                Name = "Release by GithubAPI",
                Body = "Testing release"
            };

            var result = await client.Create
                .Release(newRelease)
                .WithAssets("JgsInvoice.exe")
                .WithProgress(OnUploadChanged)
                .WithTrackingStateChanges(OnStateChanged)
                .WithCancellationToken(token)
                .Start();
            Console.WriteLine(result);
            Console.WriteLine("Done");
        }

        private static void OnStateChanged(CreatingReleaseState newValue, CreatingReleaseState oldValue)
        {
            Console.WriteLine($"STATE: {oldValue} -> {newValue}");
        }
    }
}
