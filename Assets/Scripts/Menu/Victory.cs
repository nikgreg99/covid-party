using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    [SerializeField] GameObject VictoryScreen;

    public void ShowVictoryScreen()
    {
#if !UNITY_EDITOR
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
#endif

        AudioManager.Instance.PauseLevelAudio();
        VictoryScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
