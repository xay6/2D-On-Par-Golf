using UnityEngine;
using UnityEngine.UIElements;

public class SignUpView : MonoBehaviour
{
    public event System.Action<string> OnStartGame;
    public event System.Action OnBack;

    private TextField usernameField;
    private Button startGameButton;
    private Button backButton;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        usernameField = root.Q<TextField>("usernameField");
        startGameButton = root.Q<Button>("startGameButton");
        backButton = root.Q<Button>("backButton");

        startGameButton.clicked += () => OnStartGame?.Invoke(usernameField.value);
        backButton.clicked += () => OnBack?.Invoke();
    }
}
