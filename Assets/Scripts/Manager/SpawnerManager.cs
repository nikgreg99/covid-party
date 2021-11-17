using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{

    [SerializeField] private Spawnable[] _spawnList;
    [SerializeField] private int _numMaxEnemies = 10;

    private float _totalSpawnWeight;

    private Vector3 _terrainArea;

    private float _minHeightBound;
    private float _minWidthBound;
    private float _maxHeightBound;
    private float _maxWidthBound;

    private void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        _terrainArea = terrain.terrainData.size;

        _minHeightBound = transform.position.x;
        _minWidthBound = transform.position.z;
        _maxHeightBound = _minHeightBound + _terrainArea.x;
        _maxWidthBound = _minWidthBound + _terrainArea.z;

        for (int i = 0; i < _numMaxEnemies; i++)
        {
            Spawn();
        }
    }

    void OnValidate()
    {
        _totalSpawnWeight = 0f;
        foreach (var spawnable in _spawnList)
            _totalSpawnWeight += spawnable.weight;
    }

    
    void Awake()
    {
        OnValidate();
    }

    
    public void Spawn()
    {
        float pick = Random.value * _totalSpawnWeight;
        int chosenIndex = 0;
        float cumulativeWeight = _spawnList[0].weight;

        
        while (pick > cumulativeWeight && chosenIndex < _spawnList.Length - 1)
        {
            chosenIndex++;
            cumulativeWeight += _spawnList[chosenIndex].weight;
        }

        float randomHeight = Random.Range(_minHeightBound, _maxHeightBound);
        float randomWidth = Random.Range(_minWidthBound, _maxWidthBound);

        Vector3 randomPosition = new Vector3(randomHeight, 0.0f , randomWidth);


        GameObject currentGameObject = _spawnList[chosenIndex].gameObject;
        Instantiate(currentGameObject, randomPosition, Quaternion.identity);
    }
}
