using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class LoginView : View<MetagameApplication>
    {
        Button m_LoginButton;
        Button m_SignupButton;
        Button m_BackButton;
        TextField m_UsernameField;
        TextField m_PasswordField;
        Label m_ErrorLabel;
        VisualElement m_Root;
        VisualElement m_LoadingSpinner;
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
            Debug.Log("LoginController Awake()");

            m_UsernameField = m_Root.Q<TextField>("usernameField");
            m_PasswordField = m_Root.Q<TextField>("passwordField");
            m_LoginButton = m_Root.Q<Button>("loginButton");
            m_SignupButton = m_Root.Q<Button>("signUpButton");
            m_BackButton =  m_Root.Q<Button>("backButton");
            m_ErrorLabel = m_Root.Q<Label>("errorLabel");
            
            m_LoginButton.RegisterCallback<ClickEvent>(OnClickLogin);
            m_SignupButton.RegisterCallback<ClickEvent>(OnClickSignUp);
            m_BackButton.RegisterCallback<ClickEvent>(OnClickBack);

            m_LoadingSpinner = m_Root.Q<ProgressBar>("loadingSpinner");
            m_LoadingSpinner.style.display = DisplayStyle.None;

        }

        void OnDisable()
        {
            m_LoginButton.UnregisterCallback<ClickEvent>(OnClickLogin);
            m_SignupButton.UnregisterCallback<ClickEvent>(OnClickSignUp);
            m_BackButton.UnregisterCallback<ClickEvent>(OnClickBack);
        }

        void OnClickLogin(ClickEvent evt)
        {
            Debug.Log($"Is App.View.Login null? {App.View.Login == null}");

            string username = m_UsernameField.value.Trim();
            string password = m_PasswordField.value.Trim();

            Debug.Log($"Login button clicked with username: {username}");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Username and password cannot be empty.");
                return;
            }

            Broadcast(new LoginAttemptEvent());
        }

        void OnClickSignUp(ClickEvent evt)
        {
            string username = m_UsernameField.value.Trim();
            string password = m_PasswordField.value.Trim();

            Debug.Log($"Signup button clicked with username: {username}");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Username and password cannot be empty.");
                return;
            }

            Broadcast(new SignupAttemptEvent());
        }


        void OnClickBack(ClickEvent evt)
        {
            Broadcast(new ExitLoginEvent());
        }

        internal void ShowError(string message)
        {
            if (m_ErrorLabel != null)
            {
                m_ErrorLabel.text = message;
                m_ErrorLabel.style.display = DisplayStyle.Flex;
            }
        }

        public void SetLoading(bool isLoading)
        {
            if (m_LoadingSpinner != null)
            {
                m_LoadingSpinner.style.display = isLoading ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }
    }
}
