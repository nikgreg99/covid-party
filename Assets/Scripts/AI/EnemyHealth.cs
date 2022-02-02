using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public delegate void ScoreAction(int value);
    public static ScoreAction hit;
    public static ScoreAction incrementPassive;

    [SerializeField] private GameObject healthBarPrefab;
    private GameObject healtBar;
    private Slider slider;

    [SerializeField] private int maxHealth = 100;
    private int curHealth = int.MaxValue;
    public bool IsHitAtLeastOnce { get; private set; } = false;
    public bool Infected { get { return curHealth <= 0; } }
    // Start is called before the first frame update

    private AIMovement _aIMovement;

    void Start()
    {
        _aIMovement = GetComponent<AIMovement>();
        curHealth = maxHealth;
        healtBar = Instantiate(healthBarPrefab, this.transform);
        slider = healtBar.GetComponentInChildren<Slider>();
        healtBar.transform.position += Vector3.up * 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (healtBar != null)
        {
            healtBar.transform.rotation = CameraManager.currentCamera.transform.rotation;
        }
    }

    public void TargetHit(int damage)
    {


        if (curHealth > 0)
        {
            IsHitAtLeastOnce = true;
            curHealth = Mathf.Clamp(curHealth - damage, 0, maxHealth);
            ChangeInfectedStatus(1.0f - curHealth * 1.0f / maxHealth);
            hit(5);
            if (curHealth < maxHealth)
            {
                _aIMovement.NoticeHit();
            }
            if (curHealth <= 0)
            {
                int increment = getIncrementFromAiType(_aIMovement.AIType);
                incrementPassive(increment);
                HitMarker.GetInstance().showPassiveIncrement(increment, transform.position + Vector3.up * GetComponentInChildren<CapsuleCollider>().height / 2f);
            }
        }
    }

    private int getIncrementFromAiType(AIMovement.AITypes type) => type switch
    {
        //AIMovement.AITypes.NORMAL => 1,
        AIMovement.AITypes.MASKED => 2,
        AIMovement.AITypes.VACCINATED => 2,
        AIMovement.AITypes.ANTI_CONTAMINATION => 10,
        _ => 1
    };



    private void ChangeInfectedStatus(float status)
    {
        status = Mathf.Clamp(status, 0, 1);
        if (slider != null)
        {
            slider.value = 1 - status;
            if (status == 1)
            {
                Destroy(healtBar);
                slider = null;
                healtBar = null;
            }

        }

        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.color = new Color(status, 0, 0);
        }
    }
}
