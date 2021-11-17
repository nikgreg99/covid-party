using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainGenerator : MonoBehaviour
{
 
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


    // Start is called before the first frame update
    void Start()
    {

        if (_randomGen)
        {
            _originX = Random.Range(0, 400);
            _originY = Random.Range(0, 400);
        }

        _terrain = GetComponent<Terrain>();
        this.transform.position = new Vector3(-_width / 2, 0, -_height / 2);
        _terrain.terrainData = GenerateTerrainData(_terrain.terrainData);

        SpawnGivenPlayer();
        SpwanWalls();
    }

    private void SpwanWalls()
    {
        GameObject wallParent = GameObject.FindGameObjectWithTag("WallParent");

        if (wallParent != null)
        {
            GameObject wall1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall1.transform.position = new Vector3(_width / 2, _depth, 0);
            wall1.transform.localScale = new Vector3(1, _depth * 2, _height);
            wall1.transform.SetParent(wallParent.transform);
            wall1.layer = wallParent.layer;

            GameObject wall2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall2.transform.position = new Vector3(-_width / 2, _depth, 0);
            wall2.transform.localScale = new Vector3(1, _depth * 2, _height);
            wall2.transform.SetParent(wallParent.transform);
            wall2.layer = wallParent.layer;

            GameObject wall3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall3.transform.position = new Vector3(0, _depth, _height / 2);
            wall3.transform.localScale = new Vector3(_width, _depth * 2, 1);
            wall3.transform.SetParent(wallParent.transform);
            wall3.layer = wallParent.layer;

            GameObject wall4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall4.transform.position = new Vector3(0, _depth, -_height / 2);
            wall4.transform.localScale = new Vector3(_width, _depth * 2, 1);
            wall4.transform.SetParent(wallParent.transform);
            wall4.layer = wallParent.layer;
        }
    }

    private void SpawnGivenPlayer()
    {
        if (_playerSpawnTransform != null)
        {
            _playerSpawnTransform.position = new Vector3(0, _terrain.terrainData.GetInterpolatedHeight(0, 0) + _playerSapwnYOffset, 0);
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
