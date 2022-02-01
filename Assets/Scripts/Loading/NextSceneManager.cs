using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

class NextSceneManager
{
    public static int NextScene { get; private set; } = 0;

    public static void RequestNextScene(int sceneNumber)
    {
        NextScene = sceneNumber;
        SceneManager.LoadScene("Loading");
        //SceneManager.LoadSceneAsync(sceneNumber);
    }
    public static void RequestNextScene(string sceneName)
    {
        RequestNextScene(SceneManager.GetSceneByName(sceneName).buildIndex);
    }

}
