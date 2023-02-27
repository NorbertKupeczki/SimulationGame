using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameData;

public class UnitManager : MonoBehaviour
{
    //[SerializeField] private BuildingManager buildingManager;

    [Header ("Units data")]
    [SerializeField] private List<Unit> _workerList = new List<Unit>();
    [SerializeField] private List<Unit> _farmerList = new List<Unit>();
    [SerializeField] private List<Unit> _minerList = new List<Unit>();
    [SerializeField] private List<Unit> _lumberjackList = new List<Unit>();
    [SerializeField] private List<UnitSO> _unitTypes;
    [SerializeField] private Unit _unitPrefab;

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
    }

    // Start is called before the first frame update
    void Start()
    {
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

    private bool ChangeUnitType(Unit unit, UnitType targetType)
    {
        if(unit.UnitData.UnitType != targetType)
        {
            AccessList(unit.UnitData.UnitType).Remove(unit);
            unit.UnitData = _unitTypes[(int)targetType];
            unit.name = unit.UnitData.UnitName;
            AccessList(targetType).Add(unit);
            return true;
        }
        return false;
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

    private void CreateUnitOfType(UnitType type, Vector3 position)
    {
        Unit newUnit = Instantiate(_unitPrefab, position, Quaternion.identity, gameObject.transform);
        newUnit.UnitData = _unitTypes[(int)type];
        newUnit.name = newUnit.UnitData.UnitName;
        AddUnitToList(newUnit);
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
}
