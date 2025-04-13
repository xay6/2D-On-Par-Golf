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
            Debug.Log("OnLoginAttempt called.");

            bool success = await LoginAttemptEvent.LoginHelper(evt.Username, evt.Password);

            Debug.Log($"Login attempt result: {success}");

            if (success)
            {
                App.SetCurrentUsername(evt.Username);
                View.Hide();
                App.View.AccountMenu.Show();
            }
            else
            {
                Debug.LogWarning("Login failed. Showing error message.");
                View.ShowError("Invalid login. Try again.");
            }
        }

        async void OnSignupAttempt(SignupAttemptEvent evt)
        {
            bool success = await SignupAttemptEvent.RegisterHelper(evt.Username, evt.Password);

            if (success)
            {
                View.Hide();

                // Auto-login after successful signup
                bool loginSuccess = await LoginAttemptEvent.LoginHelper(evt.Username, evt.Password);
                if (loginSuccess)
                {
                    App.View.AccountMenu.Show();
                }
                else
                {
                    View.ShowError("Signup succeeded, but login failed.");
                }
            }
        }

    }
}
