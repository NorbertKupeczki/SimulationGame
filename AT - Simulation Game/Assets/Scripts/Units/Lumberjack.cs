using System;
using System.Collections;
using UnityEngine;
using static GameData;

[CreateAssetMenu(fileName = "LumberjackBehaviourData", menuName = "Behaviours/Lumberjack Behaviour")]
public class Lumberjack : UnitBehaviour
{
    
    private WaitForSeconds _woodcuttingTime = new WaitForSeconds(WOODCUTTING_TIME);

    public override void AddResourceToStockpile(int value, Vector3 position)
    {
        _ui.StartWorldFloatingText(position, value + " Wood");
        _resourceManager.GainWood(value);
    }

    public override IEnumerator CollectingResource(Action<int> callBack)
    {
        yield return _woodcuttingTime;
        callBack(WOOD_YIELD);
        yield return null;
    }
}
