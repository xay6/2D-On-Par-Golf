using Codice.Client.BaseCommands;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class AccountView : View<MetagameApplication>
    {
        Button m_NewGameButton;
        Button m_AllLevelsButton;
        Button m_SettingsButton;
        Button m_RewardsButton;
        Button m_ChallengesButton;
        Button m_MainMenuButton;
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
            m_MainMenuButton = m_Root.Q<Button>("main-menu");
            m_ChallengesButton = m_Root.Q<Button>("challenges");

            m_NewGameButton?.RegisterCallback<ClickEvent>(OnNewGameClicked);
            m_AllLevelsButton?.RegisterCallback<ClickEvent>(OnAllLevelsClicked);
            m_SettingsButton?.RegisterCallback<ClickEvent>(OnSettingsClicked);
            m_RewardsButton?.RegisterCallback<ClickEvent>(OnRewardsClicked);
            m_MainMenuButton?.RegisterCallback<ClickEvent>(OnMainMenuClicked);
            m_ChallengesButton?.RegisterCallback<ClickEvent>(OnChallengesClicked);
        }

        void OnDisable()
        {
            m_NewGameButton?.UnregisterCallback<ClickEvent>(OnNewGameClicked);
            m_AllLevelsButton?.UnregisterCallback<ClickEvent>(OnAllLevelsClicked);
            m_SettingsButton?.UnregisterCallback<ClickEvent>(OnSettingsClicked);
            m_RewardsButton?.UnregisterCallback<ClickEvent>(OnRewardsClicked);
            m_MainMenuButton?.UnregisterCallback<ClickEvent>(OnMainMenuClicked);
        }

        void OnNewGameClicked(ClickEvent evt) => Broadcast(new StartGameEvent());
        void OnAllLevelsClicked(ClickEvent evt) => Broadcast(new EnterAllLevelsEvent());
        void OnSettingsClicked(ClickEvent evt) => Broadcast (new EnterAccountSettingsEvent());
        void OnRewardsClicked(ClickEvent evt) => SceneManager.LoadScene("RewardScene"); 
        void OnChallengesClicked(ClickEvent evt) => SceneManager.LoadScene("ChallengeLevel01"); 
        void OnMainMenuClicked(ClickEvent evt) => Broadcast(new ExitAccountEvent());

        public void InitializeClass() 
        {
            m_NewGameButton = m_Root.Q<Button>("new-game");
            m_AllLevelsButton = m_Root.Q<Button>("all-levels");
            m_SettingsButton = m_Root.Q<Button>("settings");
            m_RewardsButton = m_Root.Q<Button>("rewards");
            m_MainMenuButton = m_Root.Q<Button>("main-menu");
            m_ChallengesButton = m_Root.Q<Button>("challenges");

            m_NewGameButton?.RegisterCallback<ClickEvent>(OnNewGameClicked);
            m_AllLevelsButton?.RegisterCallback<ClickEvent>(OnAllLevelsClicked);
            m_SettingsButton?.RegisterCallback<ClickEvent>(OnSettingsClicked);
            m_RewardsButton?.RegisterCallback<ClickEvent>(OnRewardsClicked);
            m_MainMenuButton?.RegisterCallback<ClickEvent>(OnMainMenuClicked);
            m_ChallengesButton?.RegisterCallback<ClickEvent>(OnChallengesClicked);
        }
    }
}
