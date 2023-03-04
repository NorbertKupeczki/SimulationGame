using System;
using System.Collections;
using UnityEngine;
using static GameData;

[CreateAssetMenu(fileName = "MinerBehaviourData", menuName = "Behaviours/Miner Behaviour")]
public class Miner : UnitBehaviour
{
    private WaitForSeconds _minintTime = new WaitForSeconds(MINING_TIME);

    public override void AddResourceToStockpile(int value)
    {
        _resourceManager.GainOre(value);
    }

    public override IEnumerator CollectingResource(Action<int> callBack)
    {
        yield return _minintTime;
        callBack(ORE_YIELD);
        yield return null;
    }
}
