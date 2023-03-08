using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "WorkerBehaviourData", menuName = "Behaviours/Worker Behaviour")]
public class Worker : UnitBehaviour
{
    public override void AddResourceToStockpile(int value, Vector3 position)
    {
        
    }

    public override IEnumerator CollectingResource(Action<int> callBack)
    {
        yield return null;
    }
}
