using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building Data", menuName = "Building Data")]
public class BuildingSO : ScriptableObject
{
    public Sprite _buildingIcon;
    public string _buildingName;
    public GameData.BuildingType buildingType;
    public int _coinCost;
    public int _woodCost;
    public int _oreCost;
    public int _wheatCost;
    public GameObject _buildingPrefab;
}
