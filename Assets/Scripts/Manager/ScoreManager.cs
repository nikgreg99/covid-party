using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScoreManager : MonoBehaviour
{
    public delegate void PowerUpEvent(PowerupTypes type);
    public static PowerUpEvent powerUpAcquired;

    private static ScoreManager instance;

    private int _maxPoints = 40;
    private int _curPoints = 0;
    private int _tokens = 0;

    [Header("Gui Bar Assets")]
    [SerializeField] TMPro.TextMeshProUGUI _tokensText;
    [SerializeField] TMPro.TextMeshProUGUI _passiveText;
    [SerializeField] private RectTransform _barContainer;
    [SerializeField] private ContagiousBar _bar;

    [Header("Increment to required for each bar filled")]
    [SerializeField] private int _increment = 15;

    [Header("$ Income for each bar filled")]
    [SerializeField] private int _tokensIncrement = 10;

    private int _lastRegenCheck = 0;
    [Header("Regen")]
    [SerializeField] private int _curRegenQuantity = 0;
    [Tooltip("Times per second")]
    [SerializeField] private int _regenSpeed = 2;
    //times per second
    
    public static bool CanBuy(int price)
    {
        return price <= instance._tokens;
    }

    public void TryToBuy(int price, PowerupTypes powerUpType)
    {
        if (price > _tokens) return;
        _tokens -= price;
        _tokensText.text = string.Format("$ {0}", _tokens);
        powerUpAcquired(powerUpType);
    }



    private void OnEnable()
    {
        AIMovement.hit += Hit;
        AIMovement.incrementPassive += incrementPassive;
    }

    private void OnDisable()
    {
        AIMovement.hit -= Hit;
        AIMovement.incrementPassive -= incrementPassive;
    }

    public void incrementPassive(int amount)
    {
        _curRegenQuantity += amount;
        _passiveText.text = string.Format("+{0}", _curRegenQuantity);
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
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

        RectTransform passiveRect = _passiveText.gameObject.GetComponent<RectTransform>();
        Vector2 rectPos = passiveRect.anchoredPosition;
        rectPos.x = 20 + _maxPoints + 5;
        passiveRect.anchoredPosition = new Vector2(rectPos.x,rectPos.y);
    }

    private void Update()
    {
        if (Mathf.FloorToInt(Time.time * _regenSpeed) > _lastRegenCheck)
        {
            _lastRegenCheck = Mathf.FloorToInt(Time.time * _regenSpeed);
            Hit(_curRegenQuantity);
        }
        //solo per testare acquisizione oggetti
        /*if (Input.GetKeyDown(KeyCode.P))
        {
            //int pu = Random.Range(0, Enum.GetNames(typeof(PowerupTypes)).Length);
            int pu = 5;
            Debug.Log(Enum.GetName(typeof(PowerupTypes), pu));
            TryToBuy(5, (PowerupTypes)pu);
        }*/
    }
}
