using System;
using System.Collections;
using UnityEngine;
using static GameData;

public abstract class UnitBehaviour : ScriptableObject 
{
    [Header("Interaction buildings")]
    [SerializeField] public BuildingType ResourceBuilding;
    [SerializeField] public BuildingType DropOffBuilding;
    [Space]
    [Header("Unit Activities")]
    [SerializeField] public UnitActivity CollectingActivity;
    [SerializeField] public UnitActivity MovingToResourceActivity;
    [SerializeField] public UnitActivity UnloadingActivity;
    [SerializeField] public UnitActivity MovingToDropOffActivity;

    [HideInInspector] public BuildingManager _buildingManager;
    [HideInInspector] public ResourceManager _resourceManager;
    [HideInInspector] public UI _ui;

    private WaitForSeconds _unloadingTime = new WaitForSeconds(UNLOADING_TIME);

    public void InitManagers()
    {
        _buildingManager = FindObjectOfType<BuildingManager>();
        _resourceManager = FindObjectOfType<ResourceManager>();
        _ui = FindObjectOfType<UI>();
    }

    public abstract IEnumerator CollectingResource(Action<int> callBack);

    public abstract void AddResourceToStockpile(int value, Vector3 position);

    public IEnumerator UnloadResource(Action callBack)
    {
        yield return _unloadingTime;
        callBack();
        yield return null;
    }
}
