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
        }

        void OnEnterLogin(EnterLoginEvent evt)
        {
            View.Show();
        }

        void OnExitLogin(ExitLoginEvent evt)
        {
            View.Hide();
        }

        void OnLoginAttempt(LoginAttemptEvent evt)
        {
            LoginAttemptEvent.LoginHelper(evt.Username, evt.Password);
        }
    }
}
