using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class NavMeshUpdater : MonoBehaviour
{
    public GameObject GroundPlane;
    public NavMeshData NavMeshData;
    /*
    public NavMeshBuildSettings BuildSettings;
    public List<NavMeshBuildSource> Sources;
    public Bounds Bounds;
    public List<NavMeshBuildMarkup> Markups;
    */
    public NavMeshSurface Surface;

    private void Awake()
    {
        Surface = GroundPlane.GetComponent<NavMeshSurface>();
        /*
        Sources = new List<NavMeshBuildSource>();
        Markups = new List<NavMeshBuildMarkup>();
        Bounds = NavMeshData.sourceBounds;
        InitSettings();
        NavMeshBuilder.CollectSources(Bounds, LayerMask.GetMask("Ground"), NavMeshCollectGeometry.PhysicsColliders, 0, Markups, Sources);
        */
    }

    public void RefreshNavMesh()
    {
        Surface.UpdateNavMesh(Surface.navMeshData);
        //Surface.BuildNavMesh();
        //NavMeshBuilder.UpdateNavMeshData(NavMeshData,BuildSettings,Sources, Bounds);
    }
    /*
    private void InitSettings()
    {
        BuildSettings.agentRadius = 0.1f;
        BuildSettings.agentHeight = 0.4f;
        BuildSettings.agentSlope = 9.9f;
        BuildSettings.agentClimb = 0.04f;
        BuildSettings.minRegionArea = 2.0f;
        BuildSettings.tileSize = 256;
        BuildSettings.agentTypeID = 0;
        BuildSettings.maxJobWorkers = 2;

    }
    */
}
