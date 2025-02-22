using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class LoginController : Controller<MetagameApplication>
    {
        LoginView View => App.View.Login;

        void Awake()
        {
            AddListener<LoginAttemptEvent>(OnLoginAttempt);
            AddListener<ExitLoginEvent>(OnExitLogin);
        }

        void OnDestroy()
        {
            RemoveListeners();
        }

        internal override void RemoveListeners()
        {
            RemoveListener<LoginAttemptEvent>(OnLoginAttempt);
            RemoveListener<ExitLoginEvent>(OnExitLogin);
        }

        void OnLoginAttempt(LoginAttemptEvent evt)
        {
            bool success = Authenticate(evt.Username, evt.Password);
            
            if (success)
            {
                Debug.Log("Login successful!");
                SceneManager.LoadScene("MainMenu"); // Change to the appropriate scene
            }
            else
            {
                View.ShowError("Invalid username or password.");
            }
        }

        void OnExitLogin(ExitLoginEvent evt)
        {
            View.Hide();
        }

        bool Authenticate(string username, string password)
        {
            // Mock authentication - replace with actual logic
            return username == "admin" && password == "password123";
        }
    }
}
