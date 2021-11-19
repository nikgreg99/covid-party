using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{

    [SerializeField] private Spawnable[] _spawnList;
    [SerializeField] private int _numMaxEnemies = 10;
    [SerializeField] private int offsetSpawn = 5;

    private float _totalSpawnWeight;

    private Vector3 _terrainArea;
    

    private float _xMinBound;
    private float _zMinBound;
    private float _xMaxBound;
    private float _zMaxBound;

    private void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        _terrainArea = terrain.terrainData.size;

        _xMinBound = transform.position.x;
        _zMinBound = transform.position.z;
        _xMaxBound = _xMinBound + _terrainArea.x;
        _zMaxBound = _zMinBound + _terrainArea.z;

        for (int i = 0; i < _numMaxEnemies; i++)
        {
            StartCoroutine(Spawn());
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

    
    public IEnumerator Spawn()
    {
        float pick = Random.value * _totalSpawnWeight;
        int chosenIndex = 0;
        float cumulativeWeight = _spawnList[0].weight;

        
        while (pick > cumulativeWeight && chosenIndex < _spawnList.Length - 1)
        {
            chosenIndex++;
            cumulativeWeight += _spawnList[chosenIndex].weight;
        }

        float randomHeight = Random.Range(_xMinBound, _xMaxBound);
        float randomWidth = Random.Range(_zMinBound, _zMaxBound);

        
        GameObject currentGameObject = _spawnList[chosenIndex].gameObject;
        CapsuleCollider gameObjectCollider = currentGameObject.GetComponent<CapsuleCollider>();

        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit);

        Vector3 randomPosition = new Vector3(randomHeight, gameObjectCollider.height + hit.distance + offsetSpawn, randomWidth);
        Instantiate(currentGameObject, randomPosition, Quaternion.identity);
        yield return new  WaitForSeconds(0.1f);
    }
}
