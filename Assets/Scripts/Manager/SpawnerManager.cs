using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{

    [SerializeField] private Spawnable[] _spawnList;
    [SerializeField] private int _numMaxEnemies = 10;

    private float _totalSpawnWeight;
    private TerrainGenerator _terrainGenerator;
    private int _maxHeightBound;
    private int _maxWidthBound;

    private void Start()
    {
        for(int i = 0; i < _numMaxEnemies; i++)
        {
            Spawn();
        }
    }

    void OnValidate()
    {
        _terrainGenerator = GameObject.Find("Terrain").GetComponent<TerrainGenerator>();
        _maxHeightBound = _terrainGenerator.Height;
        _maxWidthBound = _terrainGenerator.Width;
        _totalSpawnWeight = 0f;
        foreach (var spawnable in _spawnList)
            _totalSpawnWeight += spawnable.weight;
    }

    
    void Awake()
    {
        OnValidate();
    }

    // Spawn an item randomly, according to the relative weights.
    public void Spawn()
    {
        // Generate a random position in the list.
        float pick = Random.value * _totalSpawnWeight;
        int chosenIndex = 0;
        float cumulativeWeight = _spawnList[0].weight;

        // Step through the list until we've accumulated more weight than this.
        // The length check is for safety in case rounding errors accumulate.
        while (pick > cumulativeWeight && chosenIndex < _spawnList.Length - 1)
        {
            chosenIndex++;
            cumulativeWeight += _spawnList[chosenIndex].weight;
        }

        float randomHeight = Random.Range(-_maxHeightBound / 2, _maxHeightBound / 2);
        float randomWidth = Random.Range(-_maxWidthBound / 2, _maxWidthBound / 2);

        Vector3 randomPosition = new Vector3(randomHeight, randomWidth);

        Instantiate(_spawnList[chosenIndex].gameObject, randomPosition, Quaternion.identity);
    }
}
