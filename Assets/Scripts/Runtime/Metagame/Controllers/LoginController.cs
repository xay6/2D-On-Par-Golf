using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using OnPar.Routers;
using Codice.Client.Common;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class LoginController : Controller<MetagameApplication>
    {
        LoginView View => App.View.Login;

        void Awake()
        {
            AddListener<EnterLoginEvent>(OnEnterLogin);
            AddListener<ExitLoginEvent>(OnExitLogin);
            AddListener<LoginAttemptEvent>(OnLoginAttempt);
            AddListener<SignupAttemptEvent>(OnSignupAttempt);
        }

        void OnDestroy()
        {
            RemoveListeners();
        }

        internal override void RemoveListeners()
        {
            RemoveListener<EnterLoginEvent>(OnEnterLogin);
            RemoveListener<ExitLoginEvent>(OnExitLogin);
            RemoveListener<LoginAttemptEvent>(OnLoginAttempt);
            RemoveListener<SignupAttemptEvent>(OnSignupAttempt);
        }

        void OnEnterLogin(EnterLoginEvent evt)
        {
            View.Show();
        }

        void OnExitLogin(ExitLoginEvent evt)
        {
            View.Hide();
        }

        async void OnLoginAttempt(LoginAttemptEvent evt)
        {
            bool success = await LoginAttemptEvent.LoginHelper(evt.Username, evt.Password);

            if (success)
            {
                View.Hide(); // Hide the login view
                App.View.AccountMenu.Show(); // Show the Account Menu View
            }
            else
            {
                View.ShowError("Invalid username or password.");
            }
        }

        async void OnSignupAttempt(SignupAttemptEvent evt)
        {
            bool success = await SignupAttemptEvent.RegisterHelper(evt.Username, evt.Password);
            //bool success = await LoginAttemptEvent.LoginHelper(evt.Username, evt.Password);

            if (success)
            {
                View.Hide(); // Hide the login view
                App.View.AccountMenu.Show(); // Show the Account Menu View
            }
            else
            {
                View.ShowError("Invalid username or password.");
            }

        }
    }
}
