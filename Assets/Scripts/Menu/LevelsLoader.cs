using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsLoader : MonoBehaviour
{

   public void loadTutorial()
    {
        SceneManager.LoadScene(1);
    }

    public void loadForestLevel()
    {
        SceneManager.LoadScene(2);
    }

    public void loadCityLevel()
    {
        SceneManager.LoadScene(3);
    }
}
