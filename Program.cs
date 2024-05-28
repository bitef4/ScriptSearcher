using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ScriptSearcher
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter search query:");
            string query = Console.ReadLine();

            string apiUrl = $"https://scriptblox.com/api/script/search?q={query}&scriptname=5&mode=free";
            var scripts = await SearchScriptsAsync(apiUrl);

            if (scripts != null)
            {
                Console.WriteLine($"Total Pages: {scripts.Result.TotalPages}");
                foreach (var script in scripts.Result.Scripts)
                {
                    Console.WriteLine($"Title: {script.Title}");
                    Console.WriteLine($"Game: {script.Game.Name}");
                    Console.WriteLine($"Script Type: {script.ScriptType}");
                    Console.WriteLine($"Views: {script.Views}");
                    Console.WriteLine($"URL: https://scriptblox.com/raw/{script.Slug}");
                    Console.WriteLine();
                }
            }
        }

        public static async Task<ScriptSearchResult> SearchScriptsAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ScriptSearchResult>(jsonResponse);
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return null;
                }
            }
        }
    }

    public class ScriptSearchResult
    {
        [JsonProperty("result")]
        public Result Result { get; set; }
    }

    public class Result
    {
        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }

        [JsonProperty("scripts")]
        public List<Script> Scripts { get; set; }
    }

    public class Script
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("game")]
        public Game Game { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }

        [JsonProperty("views")]
        public int Views { get; set; }

        [JsonProperty("scriptType")]
        public string ScriptType { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("script")]
        public string ScriptCode { get; set; }
    }

    public class Game
    {
        [JsonProperty("gameId")]
        public int GameId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
    }
}
