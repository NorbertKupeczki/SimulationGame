using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] ResourceManager _resourceManager;
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
    
    private Dictionary<GameData.BuildingType, int> _buildingLists = new Dictionary<GameData.BuildingType, int>
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
            //FindObjectOfType<NavMeshUpdater>().RefreshNavMesh();
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

    public async void FindBuildingOfType<T>(List<GameObject> collection) where T : MonoBehaviour
    {
        T[] coll = FindObjectsOfType<T>();
        foreach(var item in coll)
        {
            collection.Add(item.gameObject);
        }
        await Task.Yield();
    }

    public async Task<GameObject> GetClosestBuilding(GameData.BuildingType buildingType, Vector3 position)
    {
        if (_masterList[_buildingLists[buildingType]].Count == 1)
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
}
