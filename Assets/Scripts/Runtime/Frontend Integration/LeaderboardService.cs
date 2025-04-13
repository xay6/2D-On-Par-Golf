using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    public class LeaderboardService
    {
        private const string BaseUrl = "http://localhost:3000"; // update if hosted

        public class RedisEntry
        {
            public string value; // username
            public double score;
        }

        public class LeaderboardResponse
        {
            public bool success;
            public List<RedisEntry> data;
        }

        public static async Task<List<(string username, int score)>> FetchLeaderboard(string courseId = "global", int lower = 0, int upper = 10)
        {
            string url = $"{BaseUrl}/leaderboard/top-users?lowerlimit={lower}&upperlimit={upper}";

            UnityWebRequest request = new UnityWebRequest(url, "GET");
            request.SetRequestHeader("Content-Type", "application/json");

            var body = JsonConvert.SerializeObject(new { courseId });
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(body);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            var operation = request.SendWebRequest();
            while (!operation.isDone) await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Leaderboard fetch failed: {request.error}");
                return new List<(string, int)>();
            }

            var json = request.downloadHandler.text;
            var response = JsonConvert.DeserializeObject<LeaderboardResponse>(json);

            var results = new List<(string, int)>();
            if (response?.success == true && response.data != null)
            {
                foreach (var entry in response.data)
                {
                    results.Add((entry.value, (int)entry.score));
                }
            }

            return results;
        }
    }
}