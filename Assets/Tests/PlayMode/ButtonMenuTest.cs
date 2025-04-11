using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

public class ButtonMenuTest
{
[UnityTest]
public IEnumerator TogglePanel_ShouldTogglePanelState()
{
    var panelObject = new GameObject("TestPanel");
    panelObject.SetActive(false);

    var panelOpenerObject = new GameObject("PanelOpenerObject");
    var panelOpener = panelOpenerObject.AddComponent<PanelOpener>();
    panelOpener.Panel = panelObject;

    // First toggle: should activate
    panelOpener.TogglePanel();
    yield return null;
    Assert.IsTrue(panelObject.activeSelf, "Panel should be active after first toggle.");

    // Second toggle: should deactivate
    panelOpener.TogglePanel();
    yield return null;
    Assert.IsFalse(panelObject.activeSelf, "Panel should be inactive after second toggle.");

    Object.Destroy(panelObject);
    Object.Destroy(panelOpenerObject);
}

}
