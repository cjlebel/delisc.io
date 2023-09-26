using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

using Deliscio.Modules.BackLog;
using Deliscio.Modules.BackLog.Models;
using Deliscio.Modules.Links.Requests;

namespace Deliscio.Tools.CsvImporter;

internal partial class Program
{
    static async Task Main(string[] args)
    {
        var username = "jlebel";
        var userId = GetUserId(username);

        var connectionString = "mongodb://mongo:g%3F7%3CVd%3E9v4%3BZKk%3DJ@localhost:27018";
        var dbName = "deliscio";

        var service = new BacklogService(connectionString, dbName);

        await AddBacklogItems(service, userId);

        //await DeleteAllBacklogItems(service);
    }

    private static async Task AddBacklogItems(BacklogService service, string userId)
    {
        const string dataDir = "C:\\Temp\\MyFavs\\Data";
        const string outputDir = "C:\\Temp\\MyFavs\\Data\\Output";

        var files = Directory.GetFiles(dataDir, "*.csv");

        if (files.Length > 0)
        {
            var backlinks = new List<BacklogItem>();

            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file);

                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        var parts = SplitLineRegEx().Split(line);

                        if (parts.Length == 2)
                        {
                            var title = parts[0]?.Trim('"') ?? string.Empty;
                            var url = parts[1]?.Trim('"') ?? string.Empty;

                            // First row of each file _MAY_ contain the header. If so, skip it.
                            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(url) && title.ToLower() != "title" && url.ToLower() != "url" && !backlinks.Exists(x => x.Url == url))
                            {
                                //if (IsAcceptableHost(url))
                                //{
                                try
                                {
                                    var backlink = BacklogItem.Create(url, title, userId);

                                    backlinks.Add(backlink);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine($"There was an error while attempting to import {url}\n{e}");
                                }
                            }
                            else
                            {
                                var x = false;
                            }
                        }
                        else
                        {
                            var x = false;
                        }
                    }
                    else
                    {
                        var x = false;
                    }
                }



                //#if DEBUG
                //                // Save backlinks to a file
                //                var json = JsonSerializer.Serialize(backlinks.Select(l => new { l.Url, l.Title }).ToList());

                //                if (!Directory.Exists(outputDir))
                //                    Directory.CreateDirectory(outputDir);

                //                File.WriteAllText($"{outputDir}\\backlinks_{DateTime.Now.Ticks}.json", json);
                //#endif
            }

            if (backlinks != null && backlinks.Any())
            {
                try
                {
                    //var rslts = await service.AddBacklogItemsAsync(backlinks, CancellationToken.None);

                    Thread.Sleep(5_000);

                    var counter = 0;
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:31178");
                        foreach (var backlink in backlinks)
                        {
                            var request = new SubmitLinkRequest(backlink.Url, backlink.CreatedById);

                            try
                            {
                                var response = await client.PostAsJsonAsync("v1/links", request);
                                response.EnsureSuccessStatusCode();

                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"Successfully submitted {backlink.Url}");
                                counter++;
                            }
                            catch (HttpRequestException e)
                            {
                                Console.WriteLine($"Could not post {backlink.Url}");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Could not post {backlink.Url}");
                            }

                            Thread.Sleep(500);
                        }
                    }


                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine($"AN Exception occurred while trying to save the Back Links to the Db");

                    Console.ForegroundColor = ConsoleColor.Yellow;

                    Console.WriteLine($"Message:{Environment.NewLine}{e.Message}");
                    Console.WriteLine($"Inner:{Environment.NewLine}{e.InnerException}");
                    Console.ResetColor();

                    throw;
                }

                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine($"Successfully imported {backlinks.Count} Backlink Results");

                Console.ResetColor();
            }
        }
    }

    private static async Task DeleteAllBacklogItems(BacklogService service)
    {
        await service.RemoveAll(CancellationToken.None);

        Console.ForegroundColor = ConsoleColor.Green;

        Console.WriteLine($"Successfully deleted all Backlink Results");

        Console.ResetColor();
    }

    //private static string[] GenerateTags(string title)
    //{
    //    var tagSuggestions = new Dictionary<string, string[]>
    //    {
    //        { "agile", new [] { "agile", "scrum", "project management" } },
    //        { "apis", new [] { "api", "web services", "integration" } },
    //        { "azure", new [] { "azure", "cloud", "microsoft" } },
    //        { "best practices", new[] { "best practices", "development", "coding" } },
    //        { "cloud", new [] { "cloud computing", "scalability", "aws", "azure" } },
    //        { "c#", new [] { "csharp", "c#" } },
    //        { "containerization#", new[] { "docker", "kubernates", "k8" } },
    //        { "design patterns", new [] { "design patterns", "architecture", "best practices" } },
    //        { "development", new [] { "technology", "software", "development" } },
    //        { "devops", new [] { "devops", "automation", "cloud" } },
    //        { "docker", new [] { "docker", "containerization", "deployment" } },
    //        { "github", new []{"github"}},
    //        { "javascript", new [] { "javascript", "web", "frontend" } },
    //        { "next.js", new [] { "next.js", "react", "frontend" } },
    //        { "react", new [] { "react", "javascript", "frontend" } },
    //        { "security", new [] { "cybersecurity", "security", "hacking" } },
    //        { "software", new [] { "technology", "software", "development" } },
    //        { "stackoverflow", new[] { "stackoverflow" }},
    //        { "technology", new [] { "technology", "software", "development" } },
    //        { "visual studio", new [] {"visual studio"}},
    //        { ".net", new [] { ".net", "csharp", "microsoft", ".net", "dotnet" } },
    //    };

    //    var tags = tagSuggestions.Where(x => title.ToLower().Contains(x.Key)).SelectMany(x => x.Value).ToArray() ?? Array.Empty<string>();

    //    return tags;
    //}

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

    private static string ParseDomainFromUrl(string url)
    {
        var commonSubdomains = new List<string> { "www", "mail", "ftp" };

        if (!string.IsNullOrWhiteSpace(url))
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                string host = uri.Host.ToLower();

                //                      commonSubdomains.FirstOrDefault(subdomain => host.StartsWith(subdomain + "."));
                var matchingSubdomain = commonSubdomains.Find(subdomain => host.StartsWith(subdomain + "."));
                if (!string.IsNullOrWhiteSpace(matchingSubdomain))
                {
                    host = host.Substring(matchingSubdomain.Length + 1); // Remove the subdomain and the following dot
                }

                return host;
            }
        }

        return string.Empty;
    }

    private static bool IsAcceptableHost(string url)
    {
        var filter = new List<string>
        {
            "localhost",
            "gmail",
            "127.0.0.1",
            "192.168.",
            "mail."
        };

        if (!string.IsNullOrWhiteSpace(url))
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                string host = uri.Host.ToLower();

                return !filter.Exists(f => f.ToLower().Contains(host));
            }
        }

        return false;
    }

    [GeneratedRegex(",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)")]
    private static partial Regex SplitLineRegEx();
}