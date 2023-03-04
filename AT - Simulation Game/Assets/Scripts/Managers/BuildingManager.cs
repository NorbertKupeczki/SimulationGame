using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using static GameData;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private NavMeshUpdater _navMeshManager;
    [SerializeField] NavMeshBuildSource _buildSource;

    [Header ("Building Lists")]
    [SerializeField] private List<GameObject> _archeryRanges = new List<GameObject>();
    [SerializeField] private List<GameObject> _barracks = new List<GameObject>();
    [SerializeField] private List<GameObject> _castles = new List<GameObject>();
    [SerializeField] private List<GameObject> _houses = new List<GameObject>();
    [SerializeField] private List<GameObject> _lumberMills = new List<GameObject>();
    [SerializeField] private List<GameObject> _markets = new List<GameObject>();
    [SerializeField] private List<GameObject> _mills = new List<GameObject>();
    [SerializeField] private List<GameObject> _wells = new List<GameObject>();

    [Header ("Resources Lists")]
    [SerializeField] private List<GameObject> _farms = new List<GameObject>();
    [SerializeField] private List<GameObject> _forests = new List<GameObject>();
    [SerializeField] private List<GameObject> _mines = new List<GameObject>();
    
    private Dictionary<BuildingType, int> _buildingLists = new Dictionary<BuildingType, int>
    {
        { GameData.BuildingType.ARCHERY_RANGE, 0 },
        { GameData.BuildingType.BARRACKS, 1 },
        { GameData.BuildingType.CASTLE, 2 },
        { GameData.BuildingType.HOUSE, 3 },
        { GameData.BuildingType.LUMBER_MILL, 4 },
        { GameData.BuildingType.MARKET, 5 },
        { GameData.BuildingType.MILL, 6 },
        { GameData.BuildingType.WELL, 7 },
        { GameData.BuildingType.FARM, 8 },
        { GameData.BuildingType.FOREST, 9 },
        { GameData.BuildingType.MINE, 10 },
    };

    private List<List<GameObject>> _masterList = new List<List<GameObject>>();


    private void Awake()
    {
        _masterList.Add(_archeryRanges);
        _masterList.Add(_barracks);
        _masterList.Add(_castles);
        _masterList.Add(_houses);
        _masterList.Add(_lumberMills);
        _masterList.Add(_markets);
        _masterList.Add(_mills);
        _masterList.Add(_wells);
        _masterList.Add(_farms);
        _masterList.Add(_forests);
        _masterList.Add(_mines);

        _resourceManager = FindObjectOfType<ResourceManager>();
        _navMeshManager = FindObjectOfType<NavMeshUpdater>();
    }

    private void Start()
    {
        FindAllExistingBuildings();        
    }

    public bool ConstructBuilding(BuildingSO buildingData, Vector3 position)
    {
        if (_resourceManager.SpendResources(buildingData))
        {
            GameObject newBuilding = Instantiate(buildingData._buildingPrefab, position, Quaternion.identity);
            _navMeshManager.RefreshNavMesh();

            _masterList[_buildingLists[buildingData.buildingType]].Add(newBuilding);

            return true;
        }
        return false;
    }

    private async void FindAllExistingBuildings()
    {
        FindBuildingOfType<ArcheryRange>(_archeryRanges);
        FindBuildingOfType<Barracks>(_barracks);
        FindBuildingOfType<Castle>(_castles);
        FindBuildingOfType<House>(_houses);
        FindBuildingOfType<LumberMill>(_lumberMills);
        FindBuildingOfType<Market>(_markets);
        FindBuildingOfType<Mill>(_mills);
        FindBuildingOfType<Well>(_wells);

        FindBuildingOfType<WheatResource>(_farms);
        FindBuildingOfType<Forest>(_forests);
        FindBuildingOfType<Mine>(_mines);

        await Task.Yield();
    }

    private async void FindBuildingOfType<T>(List<GameObject> collection) where T : MonoBehaviour
    {
        T[] coll = FindObjectsOfType<T>();
        foreach(var item in coll)
        {
            collection.Add(item.gameObject);
        }
        await Task.Yield();
    }

    public async Task<GameObject> GetClosestBuilding(BuildingType buildingType, Vector3 position)
    {
        if(_masterList[_buildingLists[buildingType]].Count == 0)
        {
            return null;
        }
        else if (_masterList[_buildingLists[buildingType]].Count == 1)
        {
            return _masterList[_buildingLists[buildingType]][0];
        }
        else if (_masterList[_buildingLists[buildingType]].Count > 1)
        {
            GameObject result = new GameObject();
            float distance = float.MaxValue;

            foreach (GameObject go in _masterList[_buildingLists[buildingType]])
            {
                float currDist = Vector3.Distance(go.transform.position, position);
                if (currDist < distance)
                {
                    distance = currDist;
                    result = go;
                }
            }
            return result;
        }
        await Task.Yield();
        return null;
    }

    public void RemoveBuilding(GameObject building)
    {
        _masterList[_buildingLists[building.GetComponent<ISelectable>().GetBuildingData().buildingType]].Remove(building);
    }

    public bool CheckAvailableBuildingsOfType(BuildingType type)
    {
        return _masterList[_buildingLists[type]].Count > 0;
    }

    public async Task<GameObject> GetClosestHarvestableFarm(Vector3 position)
    {
        if (_masterList[_buildingLists[BuildingType.FARM]].Count == 0)
        {
            return null;
        }
        else if (_masterList[_buildingLists[BuildingType.FARM]].Count > 0)
        {
            int farmIndex = -1;
            float distance = float.MaxValue;

            for (int i = 0; i < _masterList[_buildingLists[BuildingType.FARM]].Count; ++i)
            {
                GameObject farm = _masterList[_buildingLists[BuildingType.FARM]][i];

                if (farm.GetComponent<WheatResource>().IsHarvestable() &&
                    Vector3.Distance(farm.transform.position, position) < distance)
                {
                    farmIndex = i;
                }
            }

            if (farmIndex > -1)
            {
                return _masterList[_buildingLists[BuildingType.FARM]][farmIndex];
            }
            else
            {
                return null;
            }
        }

        await Task.Yield();
        return null;
    }

    public async Task<GameObject> GetClosestUnregisteredFarm(Vector3 position, GameObject unit)
    {
        if (_masterList[_buildingLists[BuildingType.FARM]].Count == 0)
        {
            return null;
        }
        else if (_masterList[_buildingLists[BuildingType.FARM]].Count > 0)
        {
            int farmIndex = -1;
            float distance = float.MaxValue;

            for (int i = 0; i < _masterList[_buildingLists[BuildingType.FARM]].Count; ++i)
            {
                GameObject farm = _masterList[_buildingLists[BuildingType.FARM]][i];

                if (!farm.GetComponent<WheatResource>().HasFarmer() &&
                    Vector3.Distance(farm.transform.position, position) < distance)
                {
                    farmIndex = i;
                }
            }

            if (farmIndex > -1)
            {
                _masterList[_buildingLists[BuildingType.FARM]][farmIndex].GetComponent<WheatResource>().RegisterFarmer(unit);
                return _masterList[_buildingLists[BuildingType.FARM]][farmIndex];
            }
            else
            {
                return null;
            }
        }

        await Task.Yield();
        return null;
    }
}
