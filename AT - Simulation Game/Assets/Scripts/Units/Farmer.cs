using System;
using System.Collections;
using UnityEngine;
using static GameData;

[CreateAssetMenu(fileName = "FarmerBehaviourData", menuName = "Behaviours/Farmer Behaviour")]
public class Farmer : UnitBehaviour
{
    private WaitForSeconds _harvestingTime = new WaitForSeconds(HARVESTING_TIME);

    public override void AddResourceToStockpile(int value)
    {
        _resourceManager.GainWheat(value);
    }

    public override IEnumerator CollectingResource(Action<int> callBack)
    {
        yield return _harvestingTime;
        Debug.Log("Woodcutting Done");
        callBack(WHEAT_YIELD);
        yield return null;
    }
}
