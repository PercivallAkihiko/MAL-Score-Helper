using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Tier_List_3
{
    public class MAL
    {
        const string CLIENT_ID = "c4c2872f810d3d0bc17502beec3ceb68";
        const string VALID_CHAR = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~";
        const string UriScheme = "mal";
        const string FriendlyName = "MAL Tier Score";

        public string _CodeChallenge = string.Empty;
        public string _Url = string.Empty;
        public string _BearerToken = string.Empty;
        public string _RefreshToken = string.Empty;

        public HttpClient _Client = new HttpClient();
        public List<Anime> AnimeList;

        public Random random = new Random();

        public MAL()
        {
            _CodeChallenge = GenerateCodeChallenger(128);
            _Url = GenerateUserAuthURL();
            AnimeList = new List<Anime>();

            using (var key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\" + Global.UriScheme))
            {
                // Replace typeof(App) by the class that contains the Main method or any class located in the project that produces the exe.
                // or replace typeof(App).Assembly.Location by anything that gives the full path to the exe

                key.SetValue("", "URL:" + Global.FriendlyName);
                key.SetValue("URL Protocol", "");
                key.SetValue("authorization_code", "EMPTY");

                using (var defaultIcon = key.CreateSubKey("DefaultIcon"))
                {
                    defaultIcon.SetValue("", Global.applicationLocation + ",1");
                }

                using (var commandKey = key.CreateSubKey(@"shell\open\command"))
                {
                    commandKey.SetValue("", "\"" + Global.applicationLocation + "\" \"%1\"");
                }
            }
        }

        public string GenerateCodeChallenger(int length)
        {            
            return new string(Enumerable.Repeat(VALID_CHAR, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string GenerateUserAuthURL()
        {
            return $"https://myanimelist.net/v1/oauth2/authorize?response_type=code&client_id={CLIENT_ID}&code_challenge={_CodeChallenge}&state=RequestID42";
        }        

        public async Task GetAccessToken(string authorizationCode)
        {            
            var values = new Dictionary<string, string>{
                { "client_id", CLIENT_ID },
                { "code", authorizationCode },
                { "code_verifier", _CodeChallenge },
                { "grant_type", "authorization_code" }
            };
            var content = new FormUrlEncodedContent(values);
            HttpResponseMessage response = await _Client.PostAsync("https://myanimelist.net/v1/oauth2/token", content);
            var result = response.Content.ReadAsStringAsync().Result;            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonResult = JsonConvert.DeserializeObject<dynamic>(result)!;
                _BearerToken = jsonResult.access_token;                
                _RefreshToken = jsonResult.refresh_token;
                _Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_BearerToken}");                
            }
            else
            {
                Debug.WriteLine(result);
                throw new Exception("GetAccessToken HTTP Response went wrong. Try waiting a few minutes...");
            }
        }

        public async Task GetAnimeList()
        {
            string url = "https://api.myanimelist.net/v2/users/@me/animelist?fields=my_list_status&limit=1000&nsfw=true";
            HttpResponseMessage response = await _Client.GetAsync(url);
            var result = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {                
                Debug.WriteLine(result);
                AnimeList obj = JsonConvert.DeserializeObject<AnimeList>(result)!;
                foreach (Datum data in obj.Data)
                {
                    AnimeList.Add(data.Anime);
                }
            }
            else
            {
                Debug.WriteLine(result);
                throw new Exception("GetAnimeList HTTP Response went wrong. Try waiting a few minutes...");
            }
        }

        public async Task UpdateAnimeScore(int id, int score, string title)
        {
            var values = new Dictionary<string, string>{
                { "score", score.ToString() }
            };
            var content = new FormUrlEncodedContent(values);
            string url = $"https://api.myanimelist.net/v2/anime/{id}/my_list_status";
            HttpResponseMessage response = await _Client.PatchAsync(url, content);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Cannot update {title}.");
            }
        }
    }

    public class MainPicture
    {
        [JsonProperty("medium")]
        public string Medium { get; set; }

        [JsonProperty("large")]
        public string Large { get; set; }
    }

    public class Anime
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("main_picture")]
        public MainPicture MainPicture { get; set; }

        [JsonProperty("my_list_status")]
        public MyListStatus MyListStatus { get; set; }
    }

    public class MyListStatus
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("num_episodes_watched")]
        public int NumEpisodesWatched { get; set; }

        [JsonProperty("is_rewatching")]
        public bool IsRewatching { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
    }

    public class Paging
    {
        [JsonProperty("next")]
        public string Next { get; set; }
    }

    public class Datum
    {
        [JsonProperty("node")]
        public Anime Anime { get; set; }
    }

    public class AnimeList
    {
        [JsonProperty("data")]
        public List<Datum> Data { get; set; }

        [JsonProperty("paging")]
        public Paging Paging { get; set; }
    }

}
