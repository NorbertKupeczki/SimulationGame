using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPoint : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _buildingScript;

    public MonoBehaviour GetBuildingScript()
    {
        return _buildingScript;
    }
}
