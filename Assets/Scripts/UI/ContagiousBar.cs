using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContagiousBar : MonoBehaviour
{
    [SerializeField] private RectTransform _parentsRectTransform;
    private RectTransform _thisRectTransform;
    // Start is called before the first frame update
    void Start()
    {
        _thisRectTransform = GetComponent<RectTransform>();
        _thisRectTransform.sizeDelta = new Vector2(0, _parentsRectTransform.sizeDelta.y - 20);
    }

    public void UpdatePercentage(float fraction)
    {
        Vector2 parentSize = _parentsRectTransform.sizeDelta;
        _thisRectTransform.sizeDelta = new Vector2((parentSize.x - 20) * fraction, parentSize.y - 20);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
