using UnityEngine;
using static GameData;

public class Castle : MonoBehaviour, IBuildingInteraction, ISelectable
{
    [SerializeField] private InteractionPoint _iPoint;
    [SerializeField] private BuildingSO _buildingData;
    [SerializeField] private GameObject _castleButtons;

    private void Awake()
    {
        _castleButtons.SetActive(false);
        _iPoint = GetComponentInChildren<InteractionPoint>();
    }

    public Collider GetInteractionCollider()
    {
        return _iPoint.GetComponent<Collider>();
    }

    public Vector3 GetInteractionDestination()
    {
        return _iPoint.transform.position;
    }

    public BuildingType GetBuildingType()
    {
        return _buildingData.buildingType;
    }

    public void IsSelected()
    {
        _castleButtons.SetActive(true);
    }

    public void IsDeselected()
    {
        _castleButtons.SetActive(false);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public Transform GetInteractionPointTransform()
    {
        return _iPoint.transform;
    }

    public void DestroyBuilding()
    {
        Destroy(gameObject);
    }

    public BuildingSO GetBuildingData()
    {
        return _buildingData;
    }
}