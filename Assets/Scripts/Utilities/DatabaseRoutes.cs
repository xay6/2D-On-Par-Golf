using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Threading.Tasks;

namespace OnPar.Routers
{
    [System.Serializable]
    public class LoginRegisterResponse
    {
        public string message;
        public bool success;
    }

    public static class LoginRegister
    {
        // Post request
        public static async Task<LoginRegisterResponse> LoginRoute(string username, string password)
        {
            string url = "https://on-par-server.onrender.com/api/users/login";
            // string url = "localhost:3000/api/users/login"; // TODO: Local development string.
            string userData = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";

            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                // Build request body.
                byte[] bodyRaw = Encoding.UTF8.GetBytes(userData);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

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

                LoginRegisterResponse jsonResponse = JsonUtility.FromJson<LoginRegisterResponse>(request.downloadHandler.text);
                return jsonResponse;
            }
        }

        // Post request
        public static async Task<LoginRegisterResponse> RegisterRoute(string username, string password)
        {
            string url = "https://on-par-server.onrender.com/api/users/register";
            // string url = "localhost:3000/api/users/register"; // TODO: Local development string.
            string userData = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";

            using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
            {
                // Build request body.
                byte[] bodyRaw = Encoding.UTF8.GetBytes(userData);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

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

                LoginRegisterResponse jsonResponse = JsonUtility.FromJson<LoginRegisterResponse>(request.downloadHandler.text);
                return jsonResponse;
            }
        }
    }
}
