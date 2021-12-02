using Unity.AI.Navigation;
using UnityEngine;

public class RuntimeBake : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;

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