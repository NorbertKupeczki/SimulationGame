using UnityEngine;
using static GameData;

public class LumberMill : MonoBehaviour, IBuildingInteraction, ISelectable
{
    [SerializeField] private InteractionPoint _iPoint;
    [SerializeField] private BuildingSO _buildingData;
    [SerializeField] private GameObject _buttonsPanel;

    private void Awake()
    {
        _iPoint = GetComponentInChildren<InteractionPoint>();
    }

    private void Start()
    {
        _buttonsPanel = FindObjectOfType<BuildingsButtonManager>().GetPanelOfBuildingType(_buildingData.buildingType);
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
        _buttonsPanel.SetActive(true);
    }

    public void IsDeselected()
    {
        _buttonsPanel.SetActive(false);
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
        FindObjectOfType<BuildingManager>().RemoveBuilding(gameObject);
        Destroy(gameObject);
    }

    public BuildingSO GetBuildingData()
    {
        return _buildingData;
    }
}