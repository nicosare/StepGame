using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private GameObject pauseMenu;

    private void Update()
    {
        if (pauseMenu != null && Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
                Resume();
            else
                Pause();
        }
    }

    public void GoToScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    public void Resume()
    {
        background.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Pause()
    {
        background.SetActive(true);
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
