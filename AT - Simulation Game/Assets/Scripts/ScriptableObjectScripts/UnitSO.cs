using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameData;

[CreateAssetMenu(fileName = "New Unit Data", menuName = "Unit Data")]
public class UnitSO : ScriptableObject
{
    public string UnitName;
    public UnitType UnitType;
    public float Speed;
    [Header("Test")]
    public BuildingType BuildingType;
    public Color Color;
}
