using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class LoginController : Controller<MetagameApplication>
    {
        LoginView View => App.View.Login;

        void Awake()
        {
            AddListener<EnterLoginEvent>(OnEnterLogin);
            AddListener<ExitLoginEvent>(OnExitLogin);
        }

        void OnDestroy()
        {
            RemoveListeners();
        }

        internal override void RemoveListeners()
        {
            RemoveListener<EnterLoginEvent>(OnEnterLogin);
            RemoveListener<ExitLoginEvent>(OnExitLogin);
        }

        void OnEnterLogin(EnterLoginEvent evt)
        {
            View.Show();
        }

        void OnExitLogin(ExitLoginEvent evt)
        {
            View.Hide();
        }

        bool Authenticate(string username, string password)
        {
            // Mock authentication - replace with actual logic
            return username == "admin";
        }
    }
}
