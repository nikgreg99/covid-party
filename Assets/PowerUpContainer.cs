using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PowerUpContainer : MonoBehaviour
{
    private static List<PowerupTypes> _acquiredUniques;
    private struct Range
    {
        public int MinInclusive { get; set; }
        public int MaxExclusive { get; set; }

        public bool isInRange(int num)
        {
            return num >= MinInclusive && num < MaxExclusive;
        }
    }

    [SerializeField] private List<PowerUp> _possibilities;
    private Dictionary<PowerUp, Range> _randomRanges;

    private int rarityPoolTotal = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (_acquiredUniques == null)
        {
            _acquiredUniques = new List<PowerupTypes>();
        }
    }
    private void generatePossibilities()
    {
        _randomRanges = new Dictionary<PowerUp, Range>();
        foreach (PowerUp powerUp in _possibilities)
        {
            if (!_acquiredUniques.Contains(powerUp.PowerupType))
            {
                Range range = new Range();
                range.MinInclusive = rarityPoolTotal;
                rarityPoolTotal += powerUp.Frequence;
                range.MaxExclusive = rarityPoolTotal;

                _randomRanges.Add(powerUp, range);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenContainer()
    {
        generatePossibilities();
        int ran = Random.Range(0, rarityPoolTotal);
        foreach (KeyValuePair<PowerUp, Range> item in _randomRanges)
        {
            if (item.Value.isInRange(ran))
            {
                if (item.Key.Unique)
                {
                    _acquiredUniques.Add(item.Key.PowerupType);
                }
                Instantiate(item.Key.gameObject, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                break;
            }
        }
        Destroy(this.gameObject);
    }
}
