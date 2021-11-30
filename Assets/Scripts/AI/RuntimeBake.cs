using Unity.AI.Navigation;
using UnityEngine;

public class RuntimeBake : MonoBehaviour
{
    private NavMeshSurface surface;
    void Start()
    {
        surface = GetComponent<NavMeshSurface>();
    }

    void Bake()
    {
        surface.BuildNavMesh();
    }

    private void OnEnable()
    {
        TerrainGenerator.terrainReady += Bake;
    }

    private void OnDisable()
    {
        TerrainGenerator.terrainReady -= Bake;
    }

}