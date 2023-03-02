using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorkerBehaviourData", menuName = "Behaviours/Worker Behaviour")]
public class Worker : UnitBehaviour
{
    public override void AddResourceToStockpile(int value)
    {
        throw new NotImplementedException();
    }

    public override IEnumerator CollectingResource(Action<int> callBack)
    {
        throw new NotImplementedException();
    }
}
