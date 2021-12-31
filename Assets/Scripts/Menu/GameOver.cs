using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject GameOverUI;

    public void EndGame()
    {
#if !UNITY_EDITOR
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
#endif

        GameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
