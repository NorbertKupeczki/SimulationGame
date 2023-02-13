using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlacementMarker : MonoBehaviour
{
    [SerializeField] Material _material;
    [SerializeField] bool _selectionOK = true;

    private DecalProjector _projector;
    private int _obstacles = 0;

    private void Awake()
    {
        _material = GetComponent<DecalProjector>().material;
        _material.SetFloat("_Good", 1);
        _projector = GetComponent<DecalProjector>();
        SetProjector(false);
    }

    private void OnDisable()
    {
        _material.SetFloat("_Good", 0);
    }

    private void SelectionOK()
    {
        _material.SetFloat("_Good", 1);
    }

    private void SelectionNotOK()
    {
        _material.SetFloat("_Good", 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        ++_obstacles;
        SelectionNotOK();
    }

    private void OnTriggerExit(Collider other)
    {
        --_obstacles;
        if (_obstacles == 0) SelectionOK();
    }

    public void SetProjector(bool value)
    {
        _projector.enabled = value;
    }    
}
