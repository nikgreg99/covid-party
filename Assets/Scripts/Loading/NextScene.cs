using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI text;
    private Coroutine textCoroutine = null;


    private void Awake()
    {
        if (text != null)
        {
            textCoroutine = StartCoroutine(changeLoading());
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nextScene());
    }

    private IEnumerator nextScene()
    {
        yield return new WaitForSeconds(0.1f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(NextSceneManager.NextScene);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            /*if (text != null)
            {
                text.text = string.Format("Loading - {0}%", Mathf.CeilToInt(asyncLoad.progress * 100));
            }*/

            if (asyncLoad.progress >= 0.9f)
            {
                if (textCoroutine != null && text != null)
                {
                    StopCoroutine(textCoroutine);
                    text.text = "";
                }
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

    }

    private IEnumerator changeLoading()
    {
        string baseText = "Loading";
        while (true)
        {
            text.text = baseText;
            yield return new WaitForSeconds(0.3f);
            text.text = baseText + ".";
            yield return new WaitForSeconds(0.3f);
            text.text = baseText + "..";
            yield return new WaitForSeconds(0.3f);
            text.text = baseText + "...";
            yield return new WaitForSeconds(0.3f);
        }
        //yield return null;
    }

}
