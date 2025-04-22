using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class AllLevelsView : View<MetagameApplication>
    {
        Button m_Page1Button;
        Button m_Page2Button;
        Button m_Page3Button;
        Button m_BackButton;
        VisualElement m_LevelContainer;
        VisualElement m_Root;

        List<LevelData> m_AllLevels;
        List<LevelData> m_PendingLevels;
        int m_SelectedPage = 0;

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
            var doc = GetComponent<UIDocument>();
            m_Root = doc?.rootVisualElement;

            if (m_Root == null)
            {
                Debug.LogWarning("OnEnable: RootVisualElement not ready yet.");
                return;
            }

            QueryElements();

            if (m_PendingLevels != null)
            {
                ApplyLevels(m_PendingLevels);
                m_PendingLevels = null;
            }
        }

        public void Initialize(List<LevelData> allLevels)
        {
            var doc = GetComponent<UIDocument>();
            m_Root = doc?.rootVisualElement;

            if (m_Root == null)
            {
                Debug.LogWarning("Initialize: UXML not ready — caching levels.");
                m_PendingLevels = allLevels;
                return;
            }

            QueryElements();
            ApplyLevels(allLevels);
        }

        void QueryElements()
        {
            m_LevelContainer = m_Root.Q<VisualElement>("levelContainer");
            m_Page1Button = m_Root.Q<Button>("page1Button");
            m_Page2Button = m_Root.Q<Button>("page2Button");
            m_Page3Button = m_Root.Q<Button>("page3Button");
            m_BackButton = m_Root.Q<Button>("back-button");

            if (m_LevelContainer == null || m_Page1Button == null || m_Page2Button == null || m_Page3Button == null || m_BackButton == null)
            {
                Debug.LogError("AllLevelsView: One or more UI elements could not be found in the UXML.");
                return;
            }

            m_Page1Button.RegisterCallback<ClickEvent>(evt => SwitchPage(0));
            m_Page2Button.RegisterCallback<ClickEvent>(evt => SwitchPage(1));
            m_Page3Button.RegisterCallback<ClickEvent>(evt => SwitchPage(2));
            m_BackButton.RegisterCallback<ClickEvent>(evt => Broadcast(new ExitAllLevelsEvent()));
        }

        void ApplyLevels(List<LevelData> allLevels)
        {
            m_AllLevels = allLevels ?? new List<LevelData>();
            m_SelectedPage = 0;
            RenderPage();
        }

        void SwitchPage(int pageIndex)
        {
            m_SelectedPage = pageIndex;
            RenderPage();
        }

        void RenderPage()
        {
            m_LevelContainer.Clear();

            int start = m_SelectedPage * 6;
            int end = Mathf.Min(start + 6, m_AllLevels.Count);

            for (int i = start; i < end; i++)
            {
                var level = m_AllLevels[i];
                var button = new Button { text = $"{level.courseId}" };

                // 🔘 Visual styling to match your UXML
                button.style.width = 150;
                button.style.height = 40;
                button.style.fontSize = 8;
                button.style.color = new StyleColor(Color.white);
                button.style.backgroundColor = new StyleColor(new Color32(140, 140, 140, 255));
                button.style.borderTopLeftRadius = 15;
                button.style.borderTopRightRadius = 15;
                button.style.borderBottomLeftRadius = 15;
                button.style.borderBottomRightRadius = 15;
                button.style.marginBottom = 8;


                if (level.isUnlocked)
                {
                    int capturedId = i;
                    button.clicked += () => LoadLevel(m_AllLevels[capturedId].courseId);
                }
                else
                {
                    button.text += " (Locked)";
                    button.SetEnabled(false);
                }

                m_LevelContainer.Add(button);
            }
        }


        void LoadLevel(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}