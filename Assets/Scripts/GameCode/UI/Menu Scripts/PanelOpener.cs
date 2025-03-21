using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject Panel;
    
    public void TogglePanel(){
        if (Panel != null){
            Panel.SetActive(!Panel.activeSelf);
        }
    }
}
