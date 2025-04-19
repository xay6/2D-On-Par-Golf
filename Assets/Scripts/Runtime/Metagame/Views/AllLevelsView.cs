using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class AllLevelsView : View<MetagameApplication>
    {
        ScrollView m_LevelList;
        Button m_BackButton;
        Button m_NextButton;
        Button m_PrevButton;
        VisualElement m_Root;
        UIDocument m_UIDocument;

        List<LevelData> m_AllLevels;
        int m_LevelsPerPage = 6;
        int m_CurrentPage = 0;

        void Awake()
        {
            m_UIDocument = GetComponent<UIDocument>();
            if (m_UIDocument != null)
            {
                m_Root = m_UIDocument.rootVisualElement;
            }
        }

        public void Initialize(List<LevelData> allLevels)
        {
            m_UIDocument = GetComponent<UIDocument>();
            if (m_UIDocument != null)
            {
                m_Root = m_UIDocument.rootVisualElement;
            }

            if (allLevels == null)
            {
                Debug.LogError("Initialize failed: allLevels is null!");
                allLevels = new List<LevelData>(); // fallback
            }

            m_LevelList = m_Root.Q<ScrollView>("levelList");
            m_BackButton = m_Root.Q<Button>("back-button");
            m_NextButton = m_Root.Q<Button>("nextButton");
            m_PrevButton = m_Root.Q<Button>("prevButton");

            if (m_LevelList == null) Debug.LogError("m_LevelList is null in Initialize!");
            if (m_BackButton == null) Debug.LogError("m_BackButton is null in Initialize!");
            if (m_NextButton == null) Debug.LogError("m_NextButton is null in Initialize!");
            if (m_PrevButton == null) Debug.LogError("m_PrevButton is null in Initialize!");

            m_BackButton.clicked += () => Broadcast(new ExitAllLevelsEvent());
            m_NextButton.clicked += ShowNextPage;
            m_PrevButton.clicked += ShowPreviousPage;

            m_AllLevels = allLevels;
            m_CurrentPage = 0;
            RenderPage();
        }

        void RenderPage()
        {
            m_LevelList.Clear();

            int start = m_CurrentPage * m_LevelsPerPage;
            int end = Mathf.Min(start + m_LevelsPerPage, m_AllLevels.Count);

            for (int i = start; i < end; i++)
            {
                var level = m_AllLevels[i];
                var button = new Button();
                button.text = $"Level {level.courseId}";

                if (level.isUnlocked)
                {
                    button.SetEnabled(true);
                    int capturedId = i; // Fix closure issue
                    button.clicked += () => LoadLevel(m_AllLevels[capturedId].courseId);
                }
                else
                {
                    button.SetEnabled(false);
                    button.text += " (Locked)";
                }

                m_LevelList.Add(button);
            }

            m_PrevButton.SetEnabled(m_CurrentPage > 0);
            m_NextButton.SetEnabled((m_CurrentPage + 1) * m_LevelsPerPage < m_AllLevels.Count);
        }

        void ShowNextPage()
        {
            m_CurrentPage++;
            RenderPage();
        }

        void ShowPreviousPage()
        {
            m_CurrentPage--;
            RenderPage();
        }

        void LoadLevel(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
