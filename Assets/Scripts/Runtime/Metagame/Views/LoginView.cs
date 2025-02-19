using UnityEngine;
using UnityEngine.UIElements;

public class LoginView : MonoBehaviour
{
    public event System.Action<string> OnLogin;
    public event System.Action OnBack;

    private TextField usernameField;
    private Button loginButton;
    private Button backButton;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        usernameField = root.Q<TextField>("usernameField");
        loginButton = root.Q<Button>("loginButton");
        backButton = root.Q<Button>("backButton");

        loginButton.clicked += () => OnLogin?.Invoke(usernameField.value);
        backButton.clicked += () => OnBack?.Invoke();
    }
}
