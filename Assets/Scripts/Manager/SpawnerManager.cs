using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{

    [SerializeField] private Spawnable[] _spawnList;
    [SerializeField] private int _numMaxEnemies = 10;
    [SerializeField] private float offsetSpawn = 0.2f;

    private float _totalSpawnWeight;

    private Vector3 _terrainArea;

    private TerrainData _terrainData;

    private float _width;
    private float _height;

    private List<Vector2> _treePositions;

    public void SpawnIA()
    {
        Terrain terrain = GetComponent<Terrain>();
        _terrainData = terrain.terrainData;
        _terrainArea = _terrainData.size;

        _width = _terrainArea.x;
        _height = _terrainArea.z;

        _treePositions = new List<TreeInstance>(Terrain.activeTerrain.terrainData.treeInstances)
            .Select(e => new Vector2(e.position.x * _width, e.position.z * _height))
            .ToList();

        for (int i = 0; i < _numMaxEnemies; i++)
        {
            StartCoroutine(Spawn());
        }
    }

    private void Start()
    {

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

        float x = Random.Range(0, _width);
        float y = Random.Range(0, _height);

        while (nearTree(new Vector2(x, y), 8))
        {
            x = Random.Range(0, _width);
            y = Random.Range(0, _height);
        }

        GameObject currentGameObject = _spawnList[chosenIndex].gameObject;
        CapsuleCollider gameObjectCollider = currentGameObject.GetComponent<CapsuleCollider>();


        Vector3 randomPosition = new Vector3(x - _width / 2, _terrainData.GetInterpolatedHeight(1.0f * x / _width, 1f * y / _height) + gameObjectCollider.height / 2 + offsetSpawn, y - _height / 2);
        Instantiate(currentGameObject, randomPosition, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
    }


    private bool nearTree(Vector2 pos, float range)
    {
        return _treePositions.Where((treePos) => (pos - treePos).magnitude <= range).Any();
    }
}
