using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class ScoreManager : MonoBehaviour
{

    private Dictionary<PowerupTypes, (Color Color, int Count, bool Unique)> detainedPowerUps;
    [SerializeField] private TMPro.TextMeshProUGUI powerUpHud;
    public class ScoreException : Exception
    {
        public ScoreException() : base("Not enough tokens") { }
    }
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

    private void countPowerup(PowerUp powerUp)
    {
        PowerupTypes t = powerUp.PowerupType;
        if (detainedPowerUps.ContainsKey(t))
        {
            (Color Color, int Count, bool Unique) existing = detainedPowerUps[t];
            existing.Count++;
            detainedPowerUps[t] = existing;
        }
        else
        {
            Color c = powerUp.gameObject.GetComponentInChildren<Outline>().OutlineColor;
            detainedPowerUps[t] = (c, 1, powerUp.Unique);
        }
        updatePowerUpCountGui();
    }

    private void updatePowerUpCountGui()
    {
        string guiString = "";
        foreach (KeyValuePair<PowerupTypes, (Color Color, int Count, bool Unique)> p in detainedPowerUps)
        {
            guiString += getFormattedColoredLine(p);
        }
        powerUpHud.text = guiString;
    }

    private string getFormattedColoredLine(KeyValuePair<PowerupTypes, (Color Color, int Count, bool Unique)> p)
    {
        string colorHex = "#" + ColorUtility.ToHtmlStringRGB(p.Value.Color);
        return string.Format("<color={0}>{1}</color>{2}\n", colorHex, Enum.GetName(typeof(PowerupTypes), p.Key).ToLower(), p.Value.Unique ? "" : "   " + p.Value.Count);
    }

    private static void changeTokens(int amount)
    {
        if (amount > instance._tokens)
        {
            throw new ScoreException();
        }
        instance._tokens += amount;
        instance._tokensText.text = string.Format("$ {0}", instance._tokens);
    }

    public static void removeTokens(int price)
    {
        changeTokens(-price);
    }


    private void OnEnable()
    {
        AIMovement.hit += Hit;
        AIMovement.incrementPassive += incrementPassive;
        PlayerMovement.acquiredPowerup += countPowerup;
    }

    private void OnDisable()
    {
        AIMovement.hit -= Hit;
        AIMovement.incrementPassive -= incrementPassive;
        PlayerMovement.acquiredPowerup -= countPowerup;
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
        detainedPowerUps = new Dictionary<PowerupTypes, (Color Color, int Count, bool Unique)>();
    }

    public void Hit(int value)
    {
        _curPoints += value;
        if (_curPoints >= _maxPoints) //bar filled
        {
            _curPoints = _curPoints - _maxPoints;
            _maxPoints = Mathf.Clamp(_maxPoints + _increment, 0, 800);
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
        passiveRect.anchoredPosition = new Vector2(rectPos.x, rectPos.y);
    }

    private void Update()
    {
        if (Mathf.FloorToInt(Time.time * _regenSpeed) > _lastRegenCheck)
        {
            _lastRegenCheck = Mathf.FloorToInt(Time.time * _regenSpeed);
            Hit(_curRegenQuantity);
        }
    }
}
