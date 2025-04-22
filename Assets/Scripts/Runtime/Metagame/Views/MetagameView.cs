using OnPar.Routers;
using UnityEngine;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    /// <summary>
    /// Main view of the <see cref="MetagameApplication"></see>
    /// </summary>
    public class MetagameView : View<MetagameApplication>
    {
        internal MainMenuView MainMenu => m_MainMenuView;

        [SerializeField]
        MainMenuView m_MainMenuView;

        internal MatchmakerView Matchmaker => m_MatchmakerView;

        [SerializeField]
        MatchmakerView m_MatchmakerView;

        internal LoadingScreenView LoadingScreen => m_LoadingScreenView;

        [SerializeField]
        LoadingScreenView m_LoadingScreenView;

        internal LeaderboardView Leaderboard => m_LeaderboardView;

        [SerializeField]
        LeaderboardView m_LeaderboardView;

        internal LoginView Login => m_LoginView;

        [SerializeField]
        LoginView m_LoginView;

        internal GuestView Guest => m_GuestView;

        [SerializeField]
        GuestView m_GuestView;

        internal AccountView AccountMenu => m_AccountView;

        [SerializeField]
        AccountView m_AccountView;

        internal AccountSettingsView AccountSettings => m_AccountSettings;

        [SerializeField]
        AccountSettingsView m_AccountSettings;

        internal AllLevelsView AllLevels => m_AllLevels;

        [SerializeField]
        AllLevelsView m_AllLevels;

        void Start()
        {
            if (App.IsDedicatedServer)
            {
                OnDedicatedServerDestroyViews();
            }
            if(LoginRegister.isLoggedIn())
            {
                InitializeAccountView();
            }
        }

        void OnDedicatedServerDestroyViews()
        {
            Destroy(gameObject);
        }

        void InitializeAccountView() {
            m_MainMenuView.Hide();
            m_AccountView.Show();
            m_AccountView.enabled = false;
            m_AccountView.InitializeClass();
            m_AccountView.enabled = true;
        }
    }
}
