using OnPar.RouterHandlers;
using UnityEngine;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class AccountController : Controller<MetagameApplication>
    {
        AccountView View => App.View.AccountMenu;

        void Awake()
        {
            AddListener<EnterAccountEvent>(OnEnterAccount);
            AddListener<ExitAccountEvent>(OnExitAccount);
        }

        void OnDestroy()
        {
            RemoveListeners();
        }

        internal override void RemoveListeners()
        {
            RemoveListener<EnterAccountEvent>(OnEnterAccount);
            RemoveListener<ExitAccountEvent>(OnExitAccount);
        }

        void OnEnterAccount(EnterAccountEvent evt)
        {
            View.Show();
        }

        void OnExitAccount(ExitAccountEvent evt)
        {
            View.Hide();
            App.View.MainMenu.Show(); //Goes back to main menu
            Handlers.LogoutHandler();
        }
    }
}
