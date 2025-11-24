using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using Meta.XR.MRUtilityKit;
using System.Collections;

public class RuntimeNavMeshBuilder : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private NavMeshSurface navmeshSurface;
    void Start()
    {
        navmeshSurface = GetComponent<NavMeshSurface>();
        MRUK.Instance.RegisterSceneLoadedCallback(BuildNavmesh);
        
    }

    public void BuildNavmesh()
    {
        StartCoroutine(BuildNavmeshRoutine());
    }

    // Update is called once per frame
    public IEnumerator BuildNavmeshRoutine()
    {
        yield return new WaitForEndOfFrame();
        navmeshSurface.BuildNavMesh();
    }
}
