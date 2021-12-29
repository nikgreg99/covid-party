using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainGenerator : MonoBehaviour
{

    public delegate void TerrainAction();
    public static TerrainAction terrainReady;
 
    private Terrain _terrain;

    [SerializeField] private int _width;


    [SerializeField] private int _depth;

    [SerializeField] private int _height;

    public int Height
    {
        get;
    }


    [SerializeField] private int _scale;

    [SerializeField] private int _originX;
    [SerializeField] private int _originY;
    [SerializeField] private bool _randomGen = true;

    [SerializeField] private Transform _playerSpawnTransform;
    [SerializeField] private float _playerSapwnYOffset;

    [SerializeField] private int _dnaCount = 20;
    [SerializeField] private PowerUpContainer powerUpContainerPrefab;
    [SerializeField] private float _dnaSpawnHeight = 1f;



    // Start is called before the first frame update
    void Start()
    {

        if (_randomGen)
        {
            _originX = Random.Range(0, 400);
            _originY = Random.Range(0, 400);
        }

        TerrainData terrainCopy = Instantiate(GetComponent<Terrain>().terrainData);
        _terrain = GetComponent<Terrain>();
        this.transform.position = new Vector3(-_width / 2, 0, -_height / 2);
        TerrainData newTerrainData = GenerateTerrainData(terrainCopy);
        _terrain.terrainData = newTerrainData;
        _terrain.GetComponent<TerrainCollider>().terrainData = newTerrainData;
        terrainReady();
        SpawnGivenPlayer();
        SpawnDNAs();
        SpwanWalls();
    }

    private void SpawnDNAs()
    {
        for (int i = 0; i < _dnaCount; i++)
        {
            int x = Random.Range(0, _width);
            int y = Random.Range(0, _height);

            Instantiate(powerUpContainerPrefab, new Vector3(x - _width / 2, _terrain.terrainData.GetInterpolatedHeight(1.0f * x / _width, 1f * y / _height) + _dnaSpawnHeight, y - _height / 2), Quaternion.identity);
        }
    }

    private void SpwanWalls()
    {
        GameObject wallParent = GameObject.FindGameObjectWithTag("WallParent");

        if (wallParent != null)
        {
            GameObject wall1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall1.transform.position = new Vector3(5 + _width / 2, _depth, 0);
            wall1.transform.localScale = new Vector3(10, _depth * 2, _height);
            wall1.transform.SetParent(wallParent.transform);
            wall1.layer = wallParent.layer;

            GameObject wall2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall2.transform.position = new Vector3(-5 -_width / 2, _depth, 0);
            wall2.transform.localScale = new Vector3(10, _depth * 2, _height);
            wall2.transform.SetParent(wallParent.transform);
            wall2.layer = wallParent.layer;

            GameObject wall3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall3.transform.position = new Vector3(0, _depth, 5 + _height / 2);
            wall3.transform.localScale = new Vector3(_width, _depth * 2, 10);
            wall3.transform.SetParent(wallParent.transform);
            wall3.layer = wallParent.layer;

            GameObject wall4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall4.transform.position = new Vector3(0, _depth, -5 - _height / 2);
            wall4.transform.localScale = new Vector3(_width, _depth * 2, 10);
            wall4.transform.SetParent(wallParent.transform);
            wall4.layer = wallParent.layer;
        }
    }

    private void SpawnGivenPlayer()
    {
        if (_playerSpawnTransform != null)
        {
            _playerSpawnTransform.position = new Vector3(0, _terrain.terrainData.GetInterpolatedHeight(0.5f, 0.5f) + _playerSpawnTransform.gameObject.GetComponent<CapsuleCollider>().height / 2+0.2f, 0);
        }
    }

    private TerrainData GenerateTerrainData(TerrainData terrainData)
    {
        terrainData.size = new Vector3(_width, _depth, _height);
        terrainData.heightmapResolution = _width + 1;

        float[,] heights = GenerateHeights();

        terrainData.SetHeights(0, 0, heights);

        return terrainData;
    }

    private float[,] GenerateHeights()
    {
        float[,] heights = new float[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {

                float perlinX = (float)x / _width * _scale + _originX;
                float perlinY = (float)y / _height * _scale + _originY;

                heights[x, y] = Mathf.PerlinNoise(perlinX, perlinY);
            }

        }

        return heights;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SpawnGivenPlayer();
        }
    }

}
