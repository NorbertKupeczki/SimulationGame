using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlacementMarker : MonoBehaviour
{
    [SerializeField] Material _material;
    
    private Collider _collider;
    private DecalProjector _projector;
    private int _obstacles = 0;

    private void Awake()
    {
        _material = GetComponent<DecalProjector>().material;
        _material.SetFloat("_Good", 1);
        _projector = GetComponent<DecalProjector>();
        _collider = GetComponent<Collider>();
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

    /// <summary>
    /// Toggle the placement indicator ON/OFF.
    /// </summary>
    /// <param name="value"></param>
    public void SetProjector(bool value)
    {
        _projector.enabled = value;
        _collider.enabled = value;
        if (value)
        {
            SelectionOK();
            _obstacles = 0;
        }
    }
}
