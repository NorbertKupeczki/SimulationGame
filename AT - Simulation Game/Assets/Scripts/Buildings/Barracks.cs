using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameData;

public class Barracks : MonoBehaviour, IBuildingInteraction
{
    [SerializeField] private InteractionPoint _iPoint;
    [SerializeField] BuildingSO _buildingData;

    private void Awake()
    {
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
}