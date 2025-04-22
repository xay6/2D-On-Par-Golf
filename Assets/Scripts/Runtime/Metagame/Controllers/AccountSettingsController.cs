using UnityEngine;
using OnPar.RouterHandlers;
using OnPar.Routers;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class AccountSettingsController : Controller<MetagameApplication>
    {
        AccountSettingsView View => App.View.AccountSettings;

        void Awake()
        {
            AddListener<EnterAccountSettingsEvent>(OnEnterAccountSettings);
            AddListener<ExitAccountSettingsEvent>(OnExitAccountSettings);
        }

        void OnDestroy()
        {
            RemoveListeners();
        }

        internal override void RemoveListeners()
        {
            RemoveListener<EnterAccountSettingsEvent>(OnEnterAccountSettings);
            RemoveListener<ExitAccountSettingsEvent>(OnExitAccountSettings);
        }

        void OnEnterAccountSettings(EnterAccountSettingsEvent evt)
        {
            View.Show();
            View.enabled = false;
            View.enabled = true;
        }

        void OnExitAccountSettings(ExitAccountSettingsEvent evt)
        {
            View.Hide();
        }
    }
}
