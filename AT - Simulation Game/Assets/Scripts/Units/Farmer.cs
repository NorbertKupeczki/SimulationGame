using System;
using System.Collections;
using UnityEngine;
using static GameData;

[CreateAssetMenu(fileName = "FarmerBehaviourData", menuName = "Behaviours/Farmer Behaviour")]
public class Farmer : UnitBehaviour
{
    private WaitForSeconds _harvestingTime = new WaitForSeconds(HARVESTING_TIME);
    private WaitForSeconds _plowingTime = new WaitForSeconds(SOWING_WHEAT_TIME);

    public override void AddResourceToStockpile(int value)
    {
        _resourceManager.GainWheat(value);
    }

    public override IEnumerator CollectingResource(Action<int> callBack)
    {
        yield return _harvestingTime;
        callBack(WHEAT_YIELD);
        yield return null;
    }

    public IEnumerator PlowField(Action callBack)
    {
        yield return _plowingTime;
        callBack();
        yield return null;
    }
}
