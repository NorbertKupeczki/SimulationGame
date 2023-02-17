using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Data", menuName = "Unit Data")]
public class UnitSO : ScriptableObject
{
    public string UnitName;
    public GameData.UnitType UnitType;
    public float Speed;
}
