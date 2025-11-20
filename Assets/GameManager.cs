
using System.Collections;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class GameManager : MonoBehaviour
{
    public GameObject characterPrefab;

    IEnumerator Start()
    {
        Debug.Log("GameManager start");

        // WAIT FOR SCENE RECONSTRUCTION MESH
        GameObject envMesh = null;

        Debug.Log("Waiting for Scene Reconstruction mesh...");

        while (envMesh == null)
        {
            foreach (var mc in FindObjectsOfType<MeshCollider>())
            {
                // Skip small colliders or your own geometry
                if (mc.sharedMesh != null && mc.gameObject.name.Contains("Mesh"))
                {
                    envMesh = mc.gameObject;
                    Debug.Log("Found environment mesh: " + envMesh.name);
                    break;
                }
            }

            yield return null;
        }

        // ENSURE MESH IS ACTIVE
        if (!envMesh.activeInHierarchy)
        {
            Debug.Log("Waiting for mesh to become active...");
            while (!envMesh.activeInHierarchy)
                yield return null;
        }

        Debug.Log("Scene Reconstruction mesh ready: " + envMesh.name);

        // Spawn character
        // SpawnCharacterOnFloor();
    }


    private void SpawnCharacterOnFloor()
    {
        Vector3 origin = Camera.main.transform.position + Vector3.up * 0.3f;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 20f))
        {
            Vector3 spawnPos = hit.point + Vector3.up * 0.1f;
            Instantiate(characterPrefab, spawnPos, Quaternion.identity);

            Debug.Log("Spawned character at: " + spawnPos);
        }
        else
        {
            Debug.LogError("Raycast did NOT hit the floor. No spawn.");
        }
    }
}
