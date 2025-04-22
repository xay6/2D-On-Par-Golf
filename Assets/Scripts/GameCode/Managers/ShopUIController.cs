using UnityEngine;

public class ShopUIController : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject settingsPanel;

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        settingsPanel.SetActive(false); // hide settings while in shop
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        settingsPanel.SetActive(true); //  go back to settings
    }
}

