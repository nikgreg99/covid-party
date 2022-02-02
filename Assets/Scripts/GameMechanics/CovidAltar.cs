using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CovidAltar : MonoBehaviour
{
    // Start is called before the first frame update
    public void ShowVictoryScreen(GameObject VictoryScreen)
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
