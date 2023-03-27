using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class NavMeshUpdater : MonoBehaviour
{
    public GameObject GroundPlane;
    public NavMeshSurface Surface;

    private delegate IEnumerator LateNavRefresh();
    private LateNavRefresh _lateRefreshFunc;

    private Coroutine _lateRefreshCoroutine;

    private void Awake()
    {
        Surface = GroundPlane.GetComponent<NavMeshSurface>();
        _lateRefreshFunc = EndOfFrameRefreshNavMesh;
    }

    private void Start()
    {
        RefreshNavMesh();
    }

    public void RefreshNavMesh()
    {
        Surface.UpdateNavMesh(Surface.navMeshData);
    }

    public void RefreshNavMeshAtEndOfFrame()
    {
        _lateRefreshCoroutine = StartCoroutine(_lateRefreshFunc());
    }

    private IEnumerator EndOfFrameRefreshNavMesh()
    {
        yield return new WaitForEndOfFrame();
        RefreshNavMesh();
        yield break;
    }
}
