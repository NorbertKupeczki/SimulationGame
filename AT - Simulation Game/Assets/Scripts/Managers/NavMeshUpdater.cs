using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class NavMeshUpdater : MonoBehaviour
{
    public GameObject GroundPlane;
    public NavMeshSurface Surface;

    private void Awake()
    {
        Surface = GroundPlane.GetComponent<NavMeshSurface>();
    }

    private void Start()
    {
        RefreshNavMesh();
    }

    public void RefreshNavMesh()
    {
        Surface.UpdateNavMesh(Surface.navMeshData);
    }
}
