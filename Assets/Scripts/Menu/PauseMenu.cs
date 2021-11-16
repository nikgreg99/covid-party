using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool gameIsPaused = false;

    public GameObject PauseMenuUI;

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void backToMainMenu()
    {
        Resume();
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
#if !UNITY_EDITOR
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
#endif
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    private void Pause()
    {
#if !UNITY_EDITOR
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
#endif

        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
}
