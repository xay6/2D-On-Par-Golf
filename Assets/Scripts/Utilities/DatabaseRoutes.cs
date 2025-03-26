using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Threading.Tasks;
using OnPar.Routers;

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
        private static string token = "";
        private static string username = "";
        private static bool hasToken = false;
        // Post request
        public static async Task<LoginRegisterResponse> LoginRoute(string username, string password)
        {
            string url = "https://on-par-server.onrender.com/api/users/login";
            // string url = "localhost:3000/api/users/login"; // Local development string.
            string userData = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";

            LoginRegisterResponse res = await RequestHelper.SendRequest<LoginRegisterResponse>(url, "POST", userData);

            hasToken = true;
            token = res.token;
            return res;
        }

        // Post request
        public static async Task<LoginRegisterResponse> RegisterRoute(string username, string password)
        {
            string url = "https://on-par-server.onrender.com/api/users/register";
            // string url = "localhost:3000/api/users/register"; // Local development string.
            string userData = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";

            return await RequestHelper.SendRequest<LoginRegisterResponse>(url, "POST", userData);
        }

        public static string getToken()
        {
            return token;
        }

        public static bool isLoggedIn()
        {
            return hasToken;
        }

        public static string getUsername()
        {
            return username;
        }
    }
    public static class Scores
    {
        // Get request
        public static async Task<ScoresResponse> GetScore(string courseId, string username)
        {
            string url = "https://on-par-server.onrender.com/api/scores/get-score";
            // string url = "localhost:3000/api/users/scores/get-score"; // Local development string.
            string userData = $"{{\"courseId\":\"{courseId}\",\"username\":\"{username}\"}}";

            return await RequestHelper.SendRequest<ScoresResponse>(url, "GET", userData);
        }

        // Put request
        public static async Task<Message> AddOrUpdateScore(string courseId, string username, int score)
        {
            string url = "https://on-par-server.onrender.com/api/scores/add-update";
            // string url = "localhost:3000/api/scores/add-update"; // Local development string.
            string userData = $"{{\"courseId\":\"{courseId}\",\"username\":\"{username}\",\"score\":\"{score}\"}}";

            return await RequestHelper.SendRequest<Message>(url, "PUT", userData);
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
            return LoginRegister.LoginRoute(username, password);        }

        public static Task<LoginRegisterResponse> RegisterHandler(string username, string password)
        {
            return LoginRegister.RegisterRoute(username, password);
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
//         // string url = "https://on-par-server.onrender.com/api/users/login";
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
//         // string url = "https://on-par-server.onrender.com/api/users/register";
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
//         // string url = "https://on-par-server.onrender.com/api/scores/get-score";
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
//         // string url = "https://on-par-server.onrender.com/api/scores/add-update";
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
