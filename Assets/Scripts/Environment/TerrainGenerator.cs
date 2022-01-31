using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
//using Random = System.Random;

public class TerrainGenerator : MonoBehaviour
{

    public delegate void TerrainAction();
    public static TerrainAction terrainReady;
    public static TerrainAction playerReady;

    private Terrain _terrain;

    [SerializeField] private int _width;


    [SerializeField] private int _depth;

    [SerializeField] private int _height;

    public int Height
    {
        get;
    }


    [SerializeField] private int _scale;

    [SerializeField] private float _originX;
    [SerializeField] private float _originY;
    //[SerializeField] private bool _randomGen = true;

    [SerializeField] private Transform _playerSpawnTransform;
    [SerializeField] private float _playerSapwnYOffset;

    [SerializeField] private int _dnaCount = 20;
    [SerializeField] private int _buildingCount = 10;
    [SerializeField] private PowerUpContainer powerUpContainerPrefab;
    [SerializeField] private float _dnaSpawnHeight = 1f;

    [SerializeField] private GameObject _buildingPrefab;

    [SerializeField] private Material _wallMaterial;



    // Start is called before the first frame update
    void Start()
    {
        _originX = Random.Range(0f, 4000f);
        _originY = Random.Range(0f, 4000f);

        TerrainData terrainCopy = Instantiate(GetComponent<Terrain>().terrainData);
        _terrain = GetComponent<Terrain>();
        this.transform.position = new Vector3(-_width / 2, 0, -_height / 2);
        TerrainData newTerrainData = GenerateTerrainData(terrainCopy);
        _terrain.terrainData = newTerrainData;
        _terrain.GetComponent<TerrainCollider>().terrainData = newTerrainData;
        terrainReady();
        SpawnGivenPlayer(newTerrainData);
        SpawnDNAs();
        SpwanWalls();
        SpawnBuildings();
        GetComponent<SpawnerManager>().SpawnIA();
        playerReady();
    }

    private void SpawnBuildings()
    {
        for (int i = 0; i < _buildingCount; i++)
        {
            int buildingRadius = Mathf.CeilToInt(_buildingPrefab.GetComponentInChildren<Renderer>().bounds.size.magnitude / 2);


            int x = Random.Range(buildingRadius, _width - buildingRadius);
            int y = Random.Range(buildingRadius, _height - buildingRadius);


            Vector3 normal = _terrain.terrainData.GetInterpolatedNormal(1.0f * x / _width, 1f * y / _height);
            Quaternion normalRotation = Quaternion.FromToRotation(Vector3.up, normal);
            Quaternion ranRot = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            Quaternion buildingRotation = normalRotation * ranRot;

            Vector3 buildingPos = new Vector3(x - _width / 2, _terrain.terrainData.GetInterpolatedHeight(1.0f * x / _width, 1f * y / _height) - 1.5f, y - _height / 2);

            Instantiate(_buildingPrefab, buildingPos, buildingRotation);

            if (Random.Range(0, 2) == 1)
            {
                PowerUpContainer disposableDNA = Instantiate(powerUpContainerPrefab, buildingPos + Vector3.one * 2, Quaternion.identity);
                disposableDNA.OpenContainer();
            }


            clearZone(x, y, buildingRadius, true);
        }
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
            wall1.GetComponentInChildren<Renderer>().material = _wallMaterial;

            GameObject wall2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall2.transform.position = new Vector3(-5 - _width / 2, _depth, 0);
            wall2.transform.localScale = new Vector3(10, _depth * 2, _height);
            wall2.transform.SetParent(wallParent.transform);
            wall2.layer = wallParent.layer;
            wall2.GetComponentInChildren<Renderer>().material = _wallMaterial;

            GameObject wall3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall3.transform.position = new Vector3(0, _depth, 5 + _height / 2);
            wall3.transform.localScale = new Vector3(_width, _depth * 2, 10);
            wall3.transform.SetParent(wallParent.transform);
            wall3.layer = wallParent.layer;
            wall3.GetComponentInChildren<Renderer>().material = _wallMaterial;

            GameObject wall4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall4.transform.position = new Vector3(0, _depth, -5 - _height / 2);
            wall4.transform.localScale = new Vector3(_width, _depth * 2, 10);
            wall4.transform.SetParent(wallParent.transform);
            wall4.layer = wallParent.layer;
            wall4.GetComponentInChildren<Renderer>().material = _wallMaterial;
        }
    }

    private void SpawnGivenPlayer(TerrainData terrainData)
    {
        Vector3 spawnPos = new Vector3(
                0,
                terrainData.GetInterpolatedHeight(0.5f, 0.5f) +
                _playerSpawnTransform.gameObject.GetComponent<CapsuleCollider>().height / 2f + 2.5f,
                0);
        _playerSpawnTransform.position = spawnPos;
        _playerSpawnTransform.gameObject.SetActive(true);

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

    public void clearZone(int x, int y, int range, bool fromGrass = false, bool fromTrees = true)
    {
        Vector2 pos = new Vector2(x, y);
        Vector2 invPos = new Vector2(y, x);
        if (fromGrass)
        {
            int[,] map = _terrain.terrainData.GetDetailLayer(0, 0, _terrain.terrainData.detailWidth, _terrain.terrainData.detailHeight, 0);
            for (int i = 0; i < _terrain.terrainData.detailWidth; i++)
            {
                for (int j = 0; j < _terrain.terrainData.detailHeight; j++)
                {
                    Vector2 cur = new Vector2(i, j);
                    if ((invPos - cur).magnitude <= range)
                    {
                        //Debug.Log(cur);
                        map[i, j] = 0;
                    }
                }
            }
            for (int i = 0; i < _terrain.terrainData.detailPrototypes.Length; i++)
            {
                Terrain.activeTerrain.terrainData.SetDetailLayer(0, 0, i, map);
            }
        }

        if (fromTrees)
        {
            List<TreeInstance> treeInstances = new List<TreeInstance>(Terrain.activeTerrain.terrainData.treeInstances);
            /*treeInstances.ForEach((tree) =>
            {
                Vector2 cur = new Vector2(tree.position.x * _width, tree.position.z * _height);
                if( (pos - cur).magnitude <= range)
                {

                }
            });*/
            treeInstances.RemoveAll((tree) =>
            {
                //Debug.Log(tree.position);
                Vector2 cur = new Vector2(tree.position.x * _width, tree.position.z * _height);
                return (pos - cur).magnitude <= range;
            });
            Terrain.activeTerrain.terrainData.SetTreeInstances(treeInstances.ToArray(), true);
            Terrain.activeTerrain.GetComponent<TerrainCollider>().enabled = false;
            Terrain.activeTerrain.GetComponent<TerrainCollider>().enabled = true;
        }



    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SpawnGivenPlayer(_terrain.terrainData);
        }
    }

}
