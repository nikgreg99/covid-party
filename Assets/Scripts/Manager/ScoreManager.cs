using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _maxPoints = 40;
    private int _curPoints = 0;
    private int _tokens = 0;
    [SerializeField] private int _increment = 15;
    [SerializeField] private int _tokensIncrement = 10;
    [SerializeField] TMPro.TextMeshProUGUI _tokensText;
    [SerializeField] private RectTransform _barContainer;
    [SerializeField] private ContagiousBar _bar;

    private void OnEnable()
    {
        Projectile.SimpleHit += Hit;
    }

    private void OnDisable()
    {
        Projectile.SimpleHit -= Hit;
    }
    // Start is called before the first frame update
    void Start()
    {
        adjustContainerWidth();
    }

    public void Hit(int value)
    {
        _curPoints += value;
        if (_curPoints >= _maxPoints) //bar filled
        {
            _curPoints = _curPoints - _maxPoints;
            _maxPoints += _increment;
            adjustContainerWidth();

            BarFilled();
        }
        _bar.UpdatePercentage((float)_curPoints / (float)_maxPoints);
    }

    public void BarFilled()
    {
        _tokens += _tokensIncrement;
        _tokensText.text = string.Format("$ {0}", _tokens);
    }

    public void adjustContainerWidth()
    {
        _barContainer.sizeDelta = new Vector2(_maxPoints, _barContainer.sizeDelta.y);
    }
}
