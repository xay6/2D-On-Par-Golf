using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class AccountView : View<MetagameApplication>
    {
        Button m_NewGameButton;
        Button m_AllLevelsButton;
        Button m_SettingsButton;
        Button m_RewardsButton;
        Button m_LogoutButton;
        VisualElement m_Root;

        void Awake()
        {
            var doc = GetComponent<UIDocument>();
            if (doc != null)
            {
                m_Root = doc.rootVisualElement;
            }
        }

        void OnEnable()
        {
            m_NewGameButton = m_Root.Q<Button>("new-game");
            m_AllLevelsButton = m_Root.Q<Button>("all-levels");
            m_SettingsButton = m_Root.Q<Button>("settings");
            m_RewardsButton = m_Root.Q<Button>("rewards");
            m_LogoutButton = m_Root.Q<Button>("logout");

            m_NewGameButton?.RegisterCallback<ClickEvent>(OnNewGameClicked);
            m_AllLevelsButton?.RegisterCallback<ClickEvent>(OnAllLevelsClicked);
            m_SettingsButton?.RegisterCallback<ClickEvent>(OnSettingsClicked);
            m_RewardsButton?.RegisterCallback<ClickEvent>(OnRewardsClicked);
            m_LogoutButton?.RegisterCallback<ClickEvent>(OnLogoutClicked);
        }

        void OnDisable()
        {
            m_NewGameButton?.UnregisterCallback<ClickEvent>(OnNewGameClicked);
            m_AllLevelsButton?.UnregisterCallback<ClickEvent>(OnAllLevelsClicked);
            m_SettingsButton?.UnregisterCallback<ClickEvent>(OnSettingsClicked);
            m_RewardsButton?.UnregisterCallback<ClickEvent>(OnRewardsClicked);
            m_LogoutButton?.UnregisterCallback<ClickEvent>(OnLogoutClicked);
        }

        void OnNewGameClicked(ClickEvent evt) => Broadcast(new StartSinglePlayerModeEvent());
        void OnAllLevelsClicked(ClickEvent evt) => Broadcast(new EnterLeaderboardEvent()); // swap if needed
        void OnSettingsClicked(ClickEvent evt) => Debug.Log("Settings clicked"); // replace with actual event
        void OnRewardsClicked(ClickEvent evt) => Debug.Log("Rewards clicked"); // replace with actual event
        void OnLogoutClicked(ClickEvent evt) => Broadcast(new ExitAccountEvent());
    }
}
