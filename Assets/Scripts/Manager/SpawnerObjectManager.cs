using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerObjectManager : MonoBehaviour
{

    [SerializeField] private GameObject target;
    [SerializeField] private int units;

    private Vector3 _terrainArea;
    private TerrainData _terrainData;


    private float _xMinBound;
    private float _zMinBound;
    private float _xMaxBound;
    private float _zMaxBound;


    // Start is called before the first frame update
    void Start()
    {
        Terrain terrain = GetComponent<Terrain>();
        TerrainData terrainData = terrain.terrainData;
        _terrainArea = terrainData.size;

        _xMinBound = transform.position.x;
        _zMinBound = transform.position.z;
        _xMaxBound = _xMinBound + _terrainArea.x;
        _zMaxBound = _zMinBound + _terrainArea.z;

     
    }

    // Update is called once per frame
    void Update()
    {
        float xRandom = Random.Range(_xMinBound, _xMaxBound);
        float zRandom = Random.Range(_zMinBound, _zMaxBound);

        //Vector3 randomPosition = new Vector3(xRandom, _terrainData.GetInterpolatedHeight(randomHeight - _xMinBound, randomWidth - _zMinBound) + gameObjectCollider.height / 2 + offsetSpawn, randomWidth);
    }
}
