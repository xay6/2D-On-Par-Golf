using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Threading.Tasks;
using OnPar.Routers;
using System.Collections.Generic;
using System;

namespace OnPar.Routers
{
    [System.Serializable]
    public class LoginRegisterResponse
    {
        public string message;
        public string token;
        public bool success;
    }

    [System.Serializable]
    public class Message
    {
        public string message;
        public bool success;
    }

    [System.Serializable]
    public class Score
    {
        public string courseId;
        public string username;
        public int score;
        public bool success;
    
    }

    [System.Serializable]
    public class ScoresResponse
    {
        public string message;
        public Score userScore;
        public bool success;
    }

    [System.Serializable]
    public class TopUser
    {
        public string value;
        public int score;
        public bool success;
    }

    [System.Serializable]
    public class TopUsersResponse
    {
        public string message;
        public TopUser[] topUsers;
        public bool success;
    }

    [System.Serializable]
    public class UserItems
    {
        public string message;
        public int coinAmount;
        public string[] rewards;
        public bool success;
    }

    public static class RequestHelper
    {
        public static async Task<T> SendRequest<T>(string url, string method, string json)
        {
            using (UnityWebRequest request = new UnityWebRequest(url, method.ToUpper()))
            {
                // Build request body.
                byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                if (LoginRegister.isLoggedIn())
                    request.SetRequestHeader("Authorization", "Bearer " + LoginRegister.getToken());

                var operation = request.SendWebRequest();
                while (!operation.isDone)
                {
                    // Allows for asyncronous operation.
                    // Keeps game from freezing while waiting for the request to complete.
                    await Task.Yield();
                }

                // ConnectionError: No internet connection.
                // ProtocolError: HTTP errors.
                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + request.error);
                    // return null;
                }
                else
                {
                    Debug.Log("Response: " + request.downloadHandler.text);
                }

                return JsonUtility.FromJson<T>(request.downloadHandler.text);
            }
        }
    }

    public static class LoginRegister
    {
        // private static string token = "";
        private static string username = "";
        private static bool hasToken = false;
        // Post request
        public static async Task<LoginRegisterResponse> LoginRoute(string username, string password)
        {
            string url = "https://on-par.onrender.com/api/users/login";
            // string url = "localhost:3000/api/users/login"; // Local development string.
            string userData = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";

            LoginRegisterResponse res = await RequestHelper.SendRequest<LoginRegisterResponse>(url, "POST", userData);

            LoginRegister.username = username;
            PlayerPrefs.SetString("Username", username);
            hasToken = true;
            PlayerPrefs.SetInt("HasToken", 1);
            // token = res.token;
            PlayerPrefs.SetString("AccessToken", res.token);
            return res;
        }

        // Post request
        public static async Task<LoginRegisterResponse> RegisterRoute(string username, string password)
        {
            string url = "https://on-par.onrender.com/api/users/register";
            // string url = "localhost:3000/api/users/register"; // Local development string.
            string userData = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";

            return await RequestHelper.SendRequest<LoginRegisterResponse>(url, "POST", userData);
        }

        // Post request
        public static async Task<Message> LogoutRoute() {
            string url = "https://on-par.onrender.com/api/users/logout";

            // LoginRegister.username = "";
            hasToken = false;
            // token = "";
            PlayerPrefs.DeleteKey("AccessToken");
            PlayerPrefs.DeleteKey("Username");
            PlayerPrefs.DeleteKey("HasToken");


            return await RequestHelper.SendRequest<Message>(url, "POST", "");
        }

        public static string getToken()
        {
            return PlayerPrefs.GetString("AccessToken");
        }

        public static bool isLoggedIn()
        {
            return Convert.ToBoolean(PlayerPrefs.GetInt("HasToken"));
        }

        public static string getUsername()
        {
            return PlayerPrefs.GetString("Username");
        }
    }
    public static class Scores
    {
        // Get request
        public static async Task<ScoresResponse> GetScore(string courseId, string username)
        {
            string url = "https://on-par.onrender.com/api/scores/get-score";
            // string url = "localhost:3000/api/users/scores/get-score"; // Local development string.
            string userData = $"{{\"courseId\":\"{courseId}\",\"username\":\"{username}\"}}";

            return await RequestHelper.SendRequest<ScoresResponse>(url, "GET", userData);
        }

        // Put request
        public static async Task<Message> AddOrUpdateScore(string courseId, string username, int score)
        {
            string url = "https://on-par.onrender.com/api/scores/add-update";
            // string url = "localhost:3000/api/scores/add-update"; // Local development string.
            string userData = $"{{\"courseId\":\"{courseId}\",\"username\":\"{username}\",\"score\":{score}}}";

            return await RequestHelper.SendRequest<Message>(url, "PUT", userData);
        }

        public class AllUserScoresResponse
        {
            public bool success;
            public string message;
            public List<Score> scores;
        }
        public static async Task<AllUserScoresResponse> GetAllUserScores(string username)
        {
            string url = $"https://on-par.onrender.com/api/scores/user-all?username={username}";
            return await RequestHelper.SendRequest<AllUserScoresResponse>(url, "GET", "");
        }

    }

    public static class Leaderboard
    {
        // Get request
        public static async Task<TopUsersResponse> GetTopUsers(string courseId, int lowerLimit = 0, int upperLimit = 10)
        {
            string url = "https://on-par.onrender.com/api/leaderboard/top-users";
            string urlParams = $"?lowerlimit={lowerLimit}&upperlimit={upperLimit}";
            // string url = "localhost:3000/api/leaderboard/top-users?lowerlimit={lowerLimit}&upperlimit={upperLimit}"; // Local development string.
            string userData = $"{{\"courseId\":\"{courseId}\"}}";

            return await RequestHelper.SendRequest<TopUsersResponse>(url + urlParams, "GET", userData);
        }

        // Get request
        public static async Task<TopUser> GetUserRank(string courseId, string username)
        {
            string url = "https://on-par.onrender.com/api/leaderboard/get-rank";
            // string url = "localhost:3000/api/leaderboard/get-rank"; // Local development string.
            string userData = $"{{\"courseId\":\"{courseId}\",\"username\":\"{username}\"}}";

            return await RequestHelper.SendRequest<TopUser>(url, "GET", userData);
        }
    }

    public static class UserItem
    {
        public static async Task<UserItems> UpdateCoins(string username, int coinAmount)
        {
            string url = "https://on-par.onrender.com/api/items/update-coins";
            // string url = "localhost:3000/api/items/update-coins"; // Local development string.
            string userData = $"{{\"username\":\"{username}\",\"coinAmount\":\"{coinAmount}\"}}";

            return await RequestHelper.SendRequest<UserItems>(url, "PUT", userData);
        }

        public static async Task<UserItems> UpdateRewards(string username, string reward)
        {
            string url = "https://on-par.onrender.com/api/items/update-rewards";
            // string url = "localhost:3000/api/items/update-rewards"; // Local development string.
            string userData = $"{{\"username\":\"{username}\",\"reward\":\"{reward}\"}}";

            return await RequestHelper.SendRequest<UserItems>(url, "PUT", userData);
        }

        public static async Task<UserItems> GetCoins(string username)
        {
            string url = "https://on-par.onrender.com/api/items/get-coins";
            // string url = "localhost:3000/api/items/get-coins"; // Local development string.
            string userData = $"{{\"username\":\"{username}\"}}";

            return await RequestHelper.SendRequest<UserItems>(url, "GET", userData);
        }

        public static async Task<UserItems> GetRewards(string username)
        {
            string url = "https://on-par.onrender.com/api/items/get-rewards";
            // string url = "localhost:3000/api/items/update-rewards"; // Local development string.
            string userData = $"{{\"username\":\"{username}\"}}";

            return await RequestHelper.SendRequest<UserItems>(url, "GET", userData);
        }
    }
}

namespace OnPar.RouterHandlers
{
    public static class Handlers
    {
        /*
            Login and Register Handlers
        */
        public static Task<LoginRegisterResponse> LoginHandler(string username, string password)
        {
            return LoginRegister.LoginRoute(username, password);
        }

        public static Task<LoginRegisterResponse> RegisterHandler(string username, string password)
        {
            return LoginRegister.RegisterRoute(username, password);
        }

        public static Task<Message> LogoutHandler() 
        {
            return LoginRegister.LogoutRoute();
        }

        /*
            Score Handlers
        */
        public static Task<ScoresResponse> GetScoresHandler(string courseId, string username)
        {
            return Scores.GetScore(courseId, username);
        }

        public static Task<Message> AddOrUpdateScore(string courseId, string username, int score)
        {
            return Scores.AddOrUpdateScore(courseId, username, score);
        }

        public static Task<Scores.AllUserScoresResponse> GetAllUserScoresHandler(string username)
        {
            return Scores.GetAllUserScores(username);
        }

        /*
            Leaderboard Handlers
        */
        public static Task<TopUsersResponse> GetTopUsersHandler(string courseId, int lowerLimit, int upperLimit)
        {
            return Leaderboard.GetTopUsers(courseId, lowerLimit, upperLimit);
        }

        public static Task<TopUser> GetUserRankHandler(string courseId, string username)
        {
            return Leaderboard.GetUserRank(courseId, username);
        }

        /*
            Item Handlers
        */
        public static Task<UserItems> UpdateCoinsHandler(string username, int coinAmount)
        {
            return UserItem.UpdateCoins(username, coinAmount);
        }

        public static Task<UserItems> UpdateRewardsHandler(string username, string reward)
        {
            return UserItem.UpdateRewards(username, reward);
        }

        public static Task<UserItems> GetCoinsHandler(string username)
        {
            return UserItem.GetCoins(username);
        }

        public static Task<UserItems> GetRewardsHandler(string username)
        {
            return UserItem.GetRewards(username);
        }
    }
}


/*
    OLD ROUTES
*/
// public static class LoginRegister
// {
//     // Post request
//     public static async Task<LoginRegisterResponse> LoginRoute(string username, string password)
//     {
//         // string url = "https://on-par.onrender.com/api/users/login";
//         string url = "localhost:3000/api/users/login"; // Local development string.
//         string userData = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";

//         using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
//         {
//             // Build request body.
//             byte[] bodyRaw = Encoding.UTF8.GetBytes(userData);
//             request.uploadHandler = new UploadHandlerRaw(bodyRaw);
//             request.downloadHandler = new DownloadHandlerBuffer();
//             request.SetRequestHeader("Content-Type", "application/json");

//             var operation = request.SendWebRequest();
//             while (!operation.isDone)
//             {
//                 // Allows for asyncronous operation.
//                 // Keeps game from freezing while waiting for the request to complete.
//                 await Task.Yield();
//             }

//             // ConnectionError: No internet connection.
//             // ProtocolError: HTTP errors.
//             if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
//             {
//                 Debug.LogError("Error: " + request.error);
//                 // return null;
//             }
//             else
//             {
//                 Debug.Log("Response: " + request.downloadHandler.text);
//             }

//             LoginRegisterResponse jsonResponse = JsonUtility.FromJson<LoginRegisterResponse>(request.downloadHandler.text);
//             return jsonResponse;
//         }
//     }

//     // Post request
//     public static async Task<LoginRegisterResponse> RegisterRoute(string username, string password)
//     {
//         // string url = "https://on-par.onrender.com/api/users/register";
//         string url = "localhost:3000/api/users/register"; // Local development string.
//         string userData = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";

//         using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
//         {
//             // Build request body.
//             byte[] bodyRaw = Encoding.UTF8.GetBytes(userData);
//             request.uploadHandler = new UploadHandlerRaw(bodyRaw);
//             request.downloadHandler = new DownloadHandlerBuffer();
//             request.SetRequestHeader("Content-Type", "application/json");

//             var operation = request.SendWebRequest();
//             while (!operation.isDone)
//             {
//                 // Allows for asyncronous operation.
//                 // Keeps game from freezing while waiting for the request to complete.
//                 await Task.Yield();
//             }

//             // ConnectionError: No internet connection.
//             // ProtocolError: HTTP errors.
//             if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
//             {
//                 Debug.LogError("Error: " + request.error);
//                 // return null;
//             }
//             else
//             {
//                 Debug.Log("Response: " + request.downloadHandler.text);
//             }

//             LoginRegisterResponse jsonResponse = JsonUtility.FromJson<LoginRegisterResponse>(request.downloadHandler.text);
//             return jsonResponse;
//         }
//     }
// }
// public static class Scores
// {
//     // Get request
//     public static async Task<ScoresResponse> GetScore(string courseId, string username)
//     {
//         // string url = "https://on-par.onrender.com/api/scores/get-score";
//         string url = "localhost:3000/api/users/scores/get-score"; // Local development string.
//         string userData = $"{{\"courseId\":\"{courseId}\",\"username\":\"{username}\"}}";

//         using (UnityWebRequest request = new UnityWebRequest(url, "GET"))
//         {
//             // Build request body.
//             byte[] bodyRaw = Encoding.UTF8.GetBytes(userData);
//             request.uploadHandler = new UploadHandlerRaw(bodyRaw);
//             request.downloadHandler = new DownloadHandlerBuffer();
//             request.SetRequestHeader("Content-Type", "application/json");

//             var operation = request.SendWebRequest();
//             while (!operation.isDone)
//             {
//                 // Allows for asyncronous operation.
//                 // Keeps game from freezing while waiting for the request to complete.
//                 await Task.Yield();
//             }

//             // ConnectionError: No internet connection.
//             // ProtocolError: HTTP errors.
//             if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
//             {
//                 Debug.LogError("Error: " + request.error);
//                 // return null;
//             }
//             else
//             {
//                 Debug.Log("Response: " + request.downloadHandler.text);
//             }

//             ScoresResponse jsonResponse = JsonUtility.FromJson<ScoresResponse>(request.downloadHandler.text);
//             return jsonResponse;
//         }
//     }

//     // Put request
//     public static async Task<Message> AddOrUpdateScore(string courseId, string username, int score)
//     {
//         // string url = "https://on-par.onrender.com/api/scores/add-update";
//         string url = "localhost:3000/api/scores/add-update"; // Local development string.
//         string userData = $"{{\"courseId\":\"{courseId}\",\"username\":\"{username}\",\"score\":\"{score}\"}}";

//         using (UnityWebRequest request = new UnityWebRequest(url, "PUT"))
//         {
//             // Build request body.
//             byte[] bodyRaw = Encoding.UTF8.GetBytes(userData);
//             request.uploadHandler = new UploadHandlerRaw(bodyRaw);
//             request.downloadHandler = new DownloadHandlerBuffer();
//             request.SetRequestHeader("Content-Type", "application/json");

//             var operation = request.SendWebRequest();
//             while (!operation.isDone)
//             {
//                 // Allows for asyncronous operation.
//                 // Keeps game from freezing while waiting for the request to complete.
//                 await Task.Yield();
//             }

//             // ConnectionError: No internet connection.
//             // ProtocolError: HTTP errors.
//             if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
//             {
//                 Debug.LogError("Error: " + request.error);
//                 // return null;
//             }
//             else
//             {
//                 Debug.Log("Response: " + request.downloadHandler.text);
//             }

//             Message jsonResponse = JsonUtility.FromJson<Message>(request.downloadHandler.text);
//             return jsonResponse;
//         }
//     }
// }
