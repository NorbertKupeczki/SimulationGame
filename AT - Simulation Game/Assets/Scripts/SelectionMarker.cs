using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMarker : MonoBehaviour
{
    private Vector3 _startPosition;
    private ISelectable _selectedBuilding;

    private void Awake()
    {
        _startPosition = new Vector3(0.0f, -10.0f, 0.0f);
        transform.position = _startPosition;        
    }

    public void SetBuilding(GameObject target, ISelectable selectedInterface)
    {
        transform.position = new Vector3(target.transform.position.x, 0.003f, target.transform.position.z);
        _selectedBuilding = selectedInterface;
        _selectedBuilding.IsSelected();
    }

    public void CancelSelection()
    {
        if(_selectedBuilding != null)
        {
            transform.position = _startPosition;
            _selectedBuilding.IsDeselected();
            _selectedBuilding = null;
        }
    }

    public Transform GetSelectedBuildingTransform()
    {
        return _selectedBuilding.GetTransform();
    }

    public Transform GetInteractionPointTransform()
    {
        return _selectedBuilding.GetInteractionPointTransform();
    }
}
