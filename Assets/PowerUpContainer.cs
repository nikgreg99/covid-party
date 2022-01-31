using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PowerUpContainer : MonoBehaviour
{
    private static List<PowerupTypes> _acquiredUniques;
    private static List<PowerupTypes> AcquiredUniques { get { if (_acquiredUniques == null) _acquiredUniques = new List<PowerupTypes>(); return _acquiredUniques; } }

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
    private void generatePossibilities(bool includeUniques = true)
    {
        _randomRanges = new Dictionary<PowerUp, Range>();
        foreach (PowerUp powerUp in _possibilities)
        {
            if (includeUniques || !powerUp.Unique)
            {
                if (!AcquiredUniques.Contains(powerUp.PowerupType))
                {
                    Range range = new Range();
                    range.MinInclusive = rarityPoolTotal;
                    rarityPoolTotal += powerUp.Frequence;
                    range.MaxExclusive = rarityPoolTotal;

                    _randomRanges.Add(powerUp, range);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenContainer(bool includeUniques = true)
    {
        generatePossibilities(includeUniques);
        int ran = Random.Range(0, rarityPoolTotal);
        foreach (KeyValuePair<PowerUp, Range> item in _randomRanges)
        {
            if (item.Value.isInRange(ran))
            {
                if (item.Key.Unique)
                {
                    AcquiredUniques.Add(item.Key.PowerupType);
                }
                Instantiate(item.Key.gameObject, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
                break;
            }
        }
        Destroy(this.gameObject);
    }
}
