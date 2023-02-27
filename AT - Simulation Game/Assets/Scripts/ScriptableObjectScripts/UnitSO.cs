using UnityEngine;
using static GameData;

[CreateAssetMenu(fileName = "New Unit Data", menuName = "Unit Data")]
public class UnitSO : ScriptableObject
{
    public string UnitName;
    public UnitType UnitType;
    public float Speed;
    public Color Color;
    public UnitBehaviour UnitBehaviour;
}
