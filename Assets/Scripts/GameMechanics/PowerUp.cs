using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupTypes
{
    SHOOT_RATE_UP,
    RANGE_UP,
    SHOT_SPEED_UP,
    HOMING,
    DOUBLE_SHOT,
    SHOTGUN,
    DAMAGE_UP

}


public class PowerUp : MonoBehaviour
{
    public static int POWERUP_PRICE = 15;
    [SerializeField] private PowerupTypes _type;
    [SerializeField] private int _frequence;
    [SerializeField] private bool _unique = false;
    public bool Unique { get { return _unique; } }

    public int Frequence { get { return _frequence; } }
    public PowerupTypes PowerupType { get { return _type; } }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
