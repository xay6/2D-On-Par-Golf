using UnityEngine.UIElements;
using UnityEngine;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class GuestView : View<MetagameApplication>
    {
        Button m_Continue;
        Button m_BackButton;
        VisualElement m_Root;
        UIDocument m_UIDocument;

        void Awake()
        {
            m_UIDocument = GetComponent<UIDocument>();
            if (m_UIDocument != null)
            {
                m_Root = m_UIDocument.rootVisualElement;
            }
        }

        void OnEnable()
        {
            m_Continue = m_Root.Q<Button>("continueButton");
            m_BackButton =  m_Root.Q<Button>("backButton");
            
            m_Continue.RegisterCallback<ClickEvent>(OnClickContinue);
            m_BackButton.RegisterCallback<ClickEvent>(OnClickBack);
        }

        void OnDisable()
        {
            m_Continue.UnregisterCallback<ClickEvent>(OnClickContinue);
            m_BackButton.UnregisterCallback<ClickEvent>(OnClickBack);
        }

        void OnClickContinue(ClickEvent evt)
        {
            Broadcast(new StartGameEvent());
        }

        void OnClickBack(ClickEvent evt)
        {
            Debug.Log("🎮 Continue clicked! Broadcasting StartGameEvent...");
            Broadcast(new StartGameEvent()); // ✅ This will load Level01
        }
    }
}
