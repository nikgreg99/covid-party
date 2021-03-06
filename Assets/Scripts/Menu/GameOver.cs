using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private AudioSource gameAudio;

    public void EndGame()
    {
#if !UNITY_EDITOR
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
#endif

        AudioManager.Instance.PauseLevelAudio();
        GameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
