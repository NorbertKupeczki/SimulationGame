using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumberMill : MonoBehaviour, IBuildingInteraction
{
    [SerializeField] private InteractionPoint _iPoint;

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
}