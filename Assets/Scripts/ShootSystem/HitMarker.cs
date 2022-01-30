using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMarker : MonoBehaviour
{
    private static HitMarker instance;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static HitMarker GetInstance()
    {
        return instance;
    }

    public void showHitMarker(int damage, Vector3 pos)
    {
        StartCoroutine(hitMarker(damage, pos));
    }

    public void showPassiveIncrement(int increment, Vector3 pos)
    {
        StartCoroutine(passiveIncrement(increment, pos));
    }

    private IEnumerator passiveIncrement(int damage, Vector3 pos)
    {
        string txt = string.Format("+{0}", damage);
        GameObject go = new GameObject();
        TMPro.TextMeshPro text = go.AddComponent<TMPro.TextMeshPro>();
        text.rectTransform.sizeDelta = new Vector2(4, 4);
        text.color = Color.green;
        text.text = txt;
        text.fontSize = 10;
        text.verticalAlignment = TMPro.VerticalAlignmentOptions.Middle;
        text.alignment = TMPro.TextAlignmentOptions.Center;
        text.transform.position = pos;
        text.transform.rotation = CameraManager.currentCamera.transform.rotation;

        float animationDuration = 0.5f;
        float startTime = Time.time;
        float endTime = Time.time + animationDuration;

        while (Time.time < endTime)
        {
            text.transform.position = pos + Vector3.Lerp(Vector3.zero, Vector3.up * 3, (Time.time - startTime) / animationDuration);
            text.transform.rotation = CameraManager.currentCamera.transform.rotation;
            yield return new WaitForEndOfFrame();
        }

        Destroy(go.gameObject);
        yield return null;
    }
    private IEnumerator hitMarker(int damage, Vector3 pos)
    {
        string txt = string.Format("-{0}", damage);
        GameObject go = new GameObject();
        TMPro.TextMeshPro text = go.AddComponent<TMPro.TextMeshPro>();
        text.rectTransform.sizeDelta = new Vector2(3, 3);
        text.color = Color.red;
        text.text = txt;
        text.fontSize = 6;
        text.verticalAlignment = TMPro.VerticalAlignmentOptions.Middle;
        text.alignment = TMPro.TextAlignmentOptions.Center;
        text.transform.position = pos;
        text.transform.rotation = CameraManager.currentCamera.transform.rotation;

        float animationDuration = 0.5f;
        float startTime = Time.time;
        float endTime = Time.time + animationDuration;

        while (Time.time < endTime)
        {
            text.transform.position = pos + Vector3.Lerp(Vector3.zero, Vector3.up * 3, (Time.time - startTime) / animationDuration);
            text.transform.rotation = CameraManager.currentCamera.transform.rotation;
            yield return new WaitForEndOfFrame();
        }

        Destroy(go.gameObject);
        yield return null;
    }
}
