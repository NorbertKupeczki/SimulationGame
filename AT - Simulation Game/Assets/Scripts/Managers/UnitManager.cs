using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameData;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private BuildingManager _buildingManager;

    public Action<Transform> CreateWorker;
    public Action<Transform, UnitType> PromoteUnit;
    public Action<Transform, UnitType> DemoteUnit;

    [Header ("Units data")]
    [SerializeField] private List<Unit> _workerList = new List<Unit>();
    [SerializeField] private List<Unit> _farmerList = new List<Unit>();
    [SerializeField] private List<Unit> _minerList = new List<Unit>();
    [SerializeField] private List<Unit> _lumberjackList = new List<Unit>();
    [SerializeField] private List<UnitSO> _unitTypes;
    [SerializeField] private Unit _unitPrefab;

    private UI _ui;

    public Action UpdateThirstEvent;
    public Action UpdateFatigueEvent;

    private Dictionary<UnitType, int> _unitLists = new Dictionary<UnitType, int>
    {
        {UnitType.WORKER, 0 },
        {UnitType.FARMER, 1 },
        {UnitType.MINER, 2 },
        {UnitType.LUMBERJACK, 3 }
    };

    private List<List<Unit>> _masterList = new List<List<Unit>>(4);

    private void Awake()
    {
        _masterList.Add(_workerList);
        _masterList.Add(_farmerList);
        _masterList.Add(_minerList);
        _masterList.Add(_lumberjackList);

        CreateWorker += OnCreateWorker;
        PromoteUnit += OnPromoteUnit;
        DemoteUnit += OnDemoteUnit;
    }

    // Start is called before the first frame update
    void Start()
    {
        _buildingManager = FindObjectOfType<BuildingManager>();
        _ui = FindObjectOfType<UI>();
        
        FindAllExistingUnits();
        StartCoroutine(UpdateThirst(3.0f));
        StartCoroutine(UpdateFatigue(3.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<Unit> AccessList(UnitType unitType)
    {
        return _masterList[_unitLists[unitType]];
    }

    private void AddUnitToList(Unit unit)
    {
        AccessList(unit.UnitData.UnitType).Add(unit);
    }

    private void FindAllExistingUnits()
    {
        Unit[] allUnits = FindObjectsOfType<Unit>();

        if (allUnits.Length < 1)
        {
            return;
        }

        foreach (Unit unit in allUnits)
        {
            AddUnitToList(unit);
            unit.transform.SetParent(gameObject.transform);
            unit.name = unit.UnitData.UnitName;
            unit.GetComponent<Renderer>().material.color = unit.UnitData.Color;
        }
    }

    private Unit FindClosestUnitOfType(UnitType type, Vector3 position)
    {
        int closestIndex = 0;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < _masterList[_unitLists[type]].Count; ++i)
        {
            float currentDistance = Vector3.Distance(_masterList[_unitLists[type]][i].transform.position, position);
            if (currentDistance < closestDistance)
            {
                closestIndex = i;
                closestDistance = currentDistance;
            }
        }

        return _masterList[_unitLists[type]][closestIndex];
    }

    private void CreateUnitOfType(UnitType type, Transform spawnPosition)
    {
        Unit newUnit = Instantiate(_unitPrefab, spawnPosition.position, spawnPosition.rotation, gameObject.transform);
        newUnit.UnitData = _unitTypes[(int)type];
        newUnit.name = newUnit.UnitData.UnitName;
        newUnit.GetComponent<MeshRenderer>().material.color = newUnit.UnitData.Color;
        AddUnitToList(newUnit);
    }

    private bool ChangeUnitType(Unit unit, UnitType targetType)
    {
        if (unit.UnitData.UnitType != targetType)
        {
            AccessList(unit.UnitData.UnitType).Remove(unit);
            unit.UnitData = _unitTypes[(int)targetType];
            unit.name = unit.UnitData.UnitName;
            unit.GetComponent<Renderer>().material.color = unit.UnitData.Color;
            AccessList(targetType).Add(unit);
            unit.ResetUnitActivities();
            return true;
        }
        return false;
    }

    private IEnumerator UpdateThirst(float frequency)
    {
        WaitForSeconds delay = new WaitForSeconds(frequency);
        while (true)
        {
            yield return delay;
            UpdateThirstEvent?.Invoke();            
        }
    }

    private IEnumerator UpdateFatigue(float frequency)
    {
        WaitForSeconds delay = new WaitForSeconds(frequency);
        while (true)
        {
            yield return delay;
            UpdateFatigueEvent?.Invoke();
        }
    }

    private void OnCreateWorker(Transform spawnPosition)
    {
        CreateUnitOfType(UnitType.WORKER, spawnPosition);
    }

    private void OnPromoteUnit(Transform requestPosition, UnitType targetType)
    {
        if (CheckAvailableWorkers())
        {
            Unit unit = FindClosestUnitOfType(UnitType.WORKER, requestPosition.position);
            ChangeUnitType(unit, targetType);            
        }
    }

    private void OnDemoteUnit(Transform requestPosition, UnitType unitType)
    {
        if (CheckAvailableUnits(unitType))
        {
            Unit unit = FindClosestUnitOfType(unitType, requestPosition.position);
            ChangeUnitType(unit, UnitType.WORKER);
        }
    }

    private bool CheckAvailableWorkers()
    {
        if (AccessList(UnitType.WORKER).Count < 1)
        {
            _ui.StartFloatText("Not enough workers!");
            return false;
        }
        return true;
    }

    private bool CheckAvailableUnits(UnitType type)
    {
        if (AccessList(type).Count < 1)
        {
            _ui.StartFloatText("Not enough units!");
            return false;
        }
        return true;
    }
}
