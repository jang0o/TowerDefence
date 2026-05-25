using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject inGameMenuPanel;
    public GameObject gameOverPanel;

    public static MenuManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Project");
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void ToggleInGameMenu()
    {
        if (inGameMenuPanel != null)
        {
            inGameMenuPanel.SetActive(!inGameMenuPanel.activeSelf);
            Time.timeScale = inGameMenuPanel.activeSelf ? 0f : 1f;
        }
    }

    public void TriggerGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitApplication()
    {
        Debug.Log("Quitting application...");
        Application.Quit();
    }
}
