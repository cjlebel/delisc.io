using System.Diagnostics;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Deliscio.Apis.WebApi.Common.Requests;
using Deliscio.Core.Data.Mongo;
using Deliscio.Modules.BackLog;
using Deliscio.Modules.BackLog.Models;
using Microsoft.Extensions.Options;

namespace Deliscio.Tools.CsvImporter;

internal partial class Program
{
    static async Task Main(string[] args)
    {
#if DEBUG
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
#endif
        var username = "deliscio";
        var userId = GetUserId(username);

        var connectionString = "mongodb://mongo:g%3F7%3CVd%3E9v4%3BZKk%3DJ@localhost:27018";
        var dbName = "deliscio";

        IOptions<MongoDbOptions> options = Options.Create(new MongoDbOptions
        {
            ConnectionString = connectionString,
            DatabaseName = dbName
        });

        var service = new BacklogService(options);

        await AddBacklogItems(service, userId);

        //await DeleteAllBacklogItems(service);
#if DEBUG
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
#endif
    }

    private static async Task AddBacklogItems(BacklogService service, string userId)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:31178");

        const string dataDir = "C:\\Temp\\MyFavs\\Data";
        const string outputDir = "C:\\Temp\\MyFavs\\Data\\Output";

        //var files1 = Directory.GetFiles(dataDir, "all-links.csv");
        var files2 = Directory.GetFiles(dataDir, "phone-tablet-links.txt");

        // var files = files1.Concat(files2).ToArray();
        var files = files2;

        if (files.Length > 0)
        {
            var backlinks = new List<BacklogItem>();

            foreach (var file in files)
            {
                var isFile2 = file.Contains("phone-tablet-links.txt");

                var lines = await File.ReadAllLinesAsync(file);

                if (isFile2)
                {
                    var x = true;
                }

                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line) &&
                        !line.Equals("title,url", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string[] parts = !isFile2 ? SplitLineRegEx().Split(line) : line.Split(". ");

                        //var parts = SplitLineRegEx().Split(line);

                        if (parts[0].ToLower() != "title" && parts[0].ToLower() != "url")
                        // || parts[0].ToLower() != "url" || (parts.Length > 1 || parts[1].ToLower() != "title" || parts[1].ToLower() != "url"))
                        {
                            Uri? tmpUri = null;

                            if (!isFile2 && Uri.TryCreate(parts[0].Trim('"').Trim(), UriKind.Absolute, out tmpUri))
                                continue;

                            if (parts.Length == 2 && !Uri.TryCreate(parts[1].Trim('"').Trim(), UriKind.RelativeOrAbsolute, out tmpUri))
                                continue;


                            if (tmpUri != null)
                            {
                                if (tmpUri.IsAbsoluteUri && !string.IsNullOrWhiteSpace(tmpUri.AbsoluteUri))
                                {
                                    var url = tmpUri.AbsoluteUri;

                                    try
                                    {
                                        if (backlinks.Any(b => b.Url == url))
                                        {
                                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                                            Console.WriteLine($"{url} already exists to the queue");
                                            Console.ResetColor();
                                        }
                                        else
                                        {
                                            var backlink = BacklogItem.Create(url, string.Empty, userId);

                                            backlinks.Add(backlink);

                                            Console.ForegroundColor = ConsoleColor.Yellow;
                                            Console.WriteLine($"Added {url} to the queue");
                                            Console.ResetColor();
                                        }

                                    }
                                    catch (Exception e)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine($"Could not parse {url}");
                                        Console.ResetColor();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Could not parse {tmpUri.OriginalString}");
                                }
                            }
                        }

                    }
                }

                if (backlinks.Any())
                {
#if DEBUG
                    // Save backlinks to a file
                    var json = JsonSerializer.Serialize(
                        backlinks.OrderBy(l => l.Url.Trim('#')).Select(l => $"{l.Url}").ToList(),
                        new JsonSerializerOptions() { AllowTrailingCommas = false });

                    if (!Directory.Exists(outputDir))
                        Directory.CreateDirectory(outputDir);

                    await File.WriteAllTextAsync($"{outputDir}\\backlinks_all_filtered_{DateTime.Now.Ticks}.json",
                        json);
#endif

                    var sw = new Stopwatch();
                    sw.Start();

                    await Parallel.ForEachAsync(backlinks, new ParallelOptions { MaxDegreeOfParallelism = 3 },
                            async (backlink, token) =>
                            {
                                await CallApi(client, backlink);
                            });

                    sw.Stop();
                    Console.ForegroundColor = ConsoleColor.Green;

                    Console.WriteLine($"Successfully imported {backlinks.Count} Backlink Results in {sw.Elapsed.Seconds} seconds");

                    Console.ResetColor();
                }
            }
        }
    }

    private static async Task<bool> CallApi(HttpClient client, BacklogItem backlink)
    {
        try
        {
            var counter = 0;


            //foreach (var backlink in backlinks)
            //{
            var request = new SubmitLinkRequest(backlink.Url, backlink.CreatedById);

            try
            {
                var response = await client.PostAsJsonAsync("v1/links", request);
                response.EnsureSuccessStatusCode();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Successfully submitted {backlink.Url}");
                Console.ResetColor();
                counter++;
            }
            catch (HttpRequestException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Could not post {backlink.Url}{Environment.NewLine}Message:{e.Message}");
                Console.ResetColor();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Could not post {backlink.Url}{Environment.NewLine}Message:{e.Message}");
                Console.ResetColor();
            }

            //Thread.Sleep(500);
            //}
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"An Exception occurred while trying to save the Back Links to the Db");

            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine($"Message:{Environment.NewLine}{e.Message}");
            Console.WriteLine($"Inner:{Environment.NewLine}{e.InnerException}");
            Console.ResetColor();

            //throw;
        }

        return true;
    }

    /// <summary>
    /// Creates a unique id as a (string) GUID based on the username that was provided.
    /// </summary>
    /// <param name="username">The name of the user to create the string for.</param>
    /// <returns>A GUID as a string</returns>
    private static string GetUserId(string username)
    {
        // Create a new instance of the MD5CryptoServiceProvider object.

        // Convert the input string to a byte array and compute the hash.
        byte[] data = MD5.HashData(Encoding.Default.GetBytes(username));

        return new Guid(data).ToString();
    }

    [GeneratedRegex(",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)")]
    private static partial Regex SplitLineRegEx();
}