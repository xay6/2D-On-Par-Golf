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

        public LoginRegisterResponse Response;

        async void OnLoginAttempt(LoginAttemptEvent evt)
        {
            Debug.Log("OnLoginAttempt called.");

            bool success = await LoginAttemptEvent.LoginHelper(evt.Username, evt.Password);

            Debug.Log($"Login attempt result: {success}");

            if (LoginAttemptEvent.Login.success)
            {
                App.SetCurrentUsername(evt.Username);
                View.Hide();
                App.View.AccountMenu.Show();
            }
            else
            {
                Debug.LogWarning("Login failed. Showing error message.");
                View.ShowError(LoginAttemptEvent.Login.message);
            }
        }

        async void OnSignupAttempt(SignupAttemptEvent evt)
        {
            bool success = await SignupAttemptEvent.RegisterHelper(evt.Username, evt.Password);

            if (success)
            {

                // Auto-login after successful signup
                if (SignupAttemptEvent.Signup.success)
                {
                    App.View.AccountMenu.Show();
                    View.Hide();
                }
                else
                {
                    View.ShowError(SignupAttemptEvent.Signup.message);
                }
            }
        }

    }
}
