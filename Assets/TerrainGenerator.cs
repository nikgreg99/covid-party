using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    //public delegate void GenerationAction();
    //public static event GenerationAction generated;

    Terrain terrain;

    [SerializeField] private int _width;
    [SerializeField] private int _depth;
    [SerializeField] private int _height;

    [SerializeField] private int _originX;
    [SerializeField] private int _originY;
    [SerializeField] private int _scale;


    // Start is called before the first frame update
    void Start()
    {

    }

    private void DoGenerate()
    {
        terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrainData(terrain.terrainData);
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
        DoGenerate();
    }

}
