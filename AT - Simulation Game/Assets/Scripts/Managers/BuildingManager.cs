using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] ResourceManager _resourceManager;

    [SerializeField] private List<Mill> _mills = new List<Mill>();
    [SerializeField] private List<WheatResource> _farms = new List<WheatResource>();
    // TODO: Add all the building types as list

    private void Awake()
    {
        _resourceManager = FindObjectOfType<ResourceManager>();
    }

    private void Start()
    {
        FindAllExistingBuildings();        
    }

    public bool ConstructBuilding(BuildingSO buildingData, Vector3 position)
    {
        if (_resourceManager.SpendResources(buildingData))
        {
            Instantiate(buildingData._buildingPrefab, position, Quaternion.identity);
            return true;
        }
        return false;
    }

    private async void FindAllExistingBuildings()
    {
        FindBuildingOfType<Mill>(_mills);
        FindBuildingOfType<WheatResource>(_farms);
        // TODO: Add all the search methods to all building types
        await Task.Yield();
    }

    public async void FindBuildingOfType<T>(List<T> collection) where T : MonoBehaviour
    {
        T[] coll = FindObjectsOfType<T>();
        foreach(var item in coll)
        {
            collection.Add(item);
        }
        await Task.Yield();
    }
}
