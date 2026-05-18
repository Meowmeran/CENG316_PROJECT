using UnityEngine;
using UnityEngine.SceneManagement;

public class XRMenu : MonoBehaviour
{
    private MenuHandler menuHandler = null;
    private SceneSwitcher sceneSwitcher = null;
    void Start()
    {
        menuHandler = FindAnyObjectByType<MenuHandler>();
        sceneSwitcher = FindAnyObjectByType<SceneSwitcher>();
        if (sceneSwitcher == null)
        {
            sceneSwitcher = new GameObject("SceneSwitcher").AddComponent<SceneSwitcher>();
        }
    }
    [ContextMenu("Main Menu")]
    public void LoadMainMenu()
    {
        sceneSwitcher.LoadScene(0);
    }

    [ContextMenu("Level 1")]
    public void LoadLevel1()
    {
        sceneSwitcher.LoadScene(1);
    }
    [ContextMenu("Level 2")]
    public void LoadLevel2()
    {
        sceneSwitcher.LoadScene(2);
    }
    [ContextMenu("Level 3")]
    public void LoadLevel3()
    {
        sceneSwitcher.LoadScene(3);
    }
    [ContextMenu("Level 4")]
    public void LoadLevel4()
    {
        sceneSwitcher.LoadScene(4);
    }
    [ContextMenu("Credits")]
    public void LoadLevel5()
    {
        sceneSwitcher.LoadScene(5); // Credits
    }
    [ContextMenu("Quit")]
    public void Quit()
    {
        sceneSwitcher.QuitGame();
    }
}