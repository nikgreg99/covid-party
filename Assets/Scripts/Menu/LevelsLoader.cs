using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsLoader : MonoBehaviour
{

    public void loadTutorial()
    {
        NextSceneManager.RequestNextScene(1);
    }

    public void loadForestLevel()
    {
        NextSceneManager.RequestNextScene(2);
    }
}
