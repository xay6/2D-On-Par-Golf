using UnityEngine;
using UnityEngine.UIElements;

namespace Unity.Template.Multiplayer.NGO.Runtime
{
    internal class LoginView : View<MetagameApplication>
    {
        Button m_LoginButton;
        Button m_SignupButton;
        Button m_BackButton;
        Button m_TogglePasswordVisibilityButton;
        bool m_IsPasswordVisible = false;
        TextField m_UsernameField;
        TextField m_PasswordField;
        Label m_ErrorLabel;
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
            Debug.Log("LoginView OnEnable()");

            // Always refresh the rootVisualElement in case it's been recreated
            m_UIDocument = GetComponent<UIDocument>();
            if (m_UIDocument != null)
            {
                m_Root = m_UIDocument.rootVisualElement;
            }

            m_UsernameField = m_Root.Q<TextField>("usernameField");
            m_PasswordField = m_Root.Q<TextField>("passwordField");
            m_LoginButton = m_Root.Q<Button>("loginButton");
            m_SignupButton = m_Root.Q<Button>("signUpButton");
            m_BackButton = m_Root.Q<Button>("backButton");
            m_ErrorLabel = m_Root.Q<Label>("errorLabel");

            m_TogglePasswordVisibilityButton = m_Root.Q<Button>("togglePasswordVisibilityButton");

            m_LoginButton.RegisterCallback<ClickEvent>(OnClickLogin);
            m_SignupButton.RegisterCallback<ClickEvent>(OnClickSignUp);
            m_BackButton.RegisterCallback<ClickEvent>(OnClickBack);
            m_TogglePasswordVisibilityButton.RegisterCallback<ClickEvent>(OnTogglePasswordVisibility);

            m_PasswordField.isPasswordField = !m_IsPasswordVisible;

        }

        void OnDisable()
        {
            m_LoginButton.UnregisterCallback<ClickEvent>(OnClickLogin);
            m_SignupButton.UnregisterCallback<ClickEvent>(OnClickSignUp);
            m_BackButton.UnregisterCallback<ClickEvent>(OnClickBack);
            m_TogglePasswordVisibilityButton.UnregisterCallback<ClickEvent>(OnTogglePasswordVisibility);

            m_IsPasswordVisible = false; // reset for next load
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

             Broadcast(new LoginAttemptEvent(username, password));
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

            Broadcast(new SignupAttemptEvent(username, password));
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

        void OnTogglePasswordVisibility(ClickEvent evt)
        {
            m_IsPasswordVisible = !m_IsPasswordVisible;
            m_PasswordField.isPasswordField = !m_IsPasswordVisible;
        }
    }
}
