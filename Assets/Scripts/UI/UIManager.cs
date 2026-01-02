using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject helpPanel;
    public KeyCode toggleHelpKey = KeyCode.H;

    private void Update()
    {
        if (helpPanel == null) return;

        if (Input.GetKeyDown(toggleHelpKey))
            helpPanel.SetActive(!helpPanel.activeSelf);
    }
}
