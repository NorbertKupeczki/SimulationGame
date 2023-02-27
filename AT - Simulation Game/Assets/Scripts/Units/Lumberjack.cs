using System;
using System.Collections;
using UnityEngine;
using static GameData;

[CreateAssetMenu(fileName = "LumberjackBehaviourData", menuName = "Behaviours/Lumberjack Behaviour")]
public class Lumberjack : UnitBehaviour
{
    
    private WaitForSeconds _woodcuttingTime = new WaitForSeconds(WOODCUTTING_TIME);

    public override void AddResourceToStockpile(int value)
    {
        _resourceManager.GainWood(value);
    }

    public override IEnumerator CollectingResource(Action<int> callBack)
    {
        yield return _woodcuttingTime;
        Debug.Log("Woodcutting Done");
        callBack(10);
        yield return null;
    }    
}
