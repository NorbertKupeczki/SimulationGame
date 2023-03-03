using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static GameData;

public class Unit : MonoBehaviour
{
    [field: Header ("Unit info")]
    [field:SerializeField] public UnitSO UnitData { get; set; }
    [field:SerializeField] public UnitActivity CurrentActivity { get; private set; }

    [field:Space]
    [field:Header("Unit stats")]
    [field:SerializeField] public float Energy { get; private set; }
    [field:SerializeField] public float Water { get; private set; }
    [field:SerializeField] public int Resources { get; private set; }

    [Header ("Interactions")]
    [SerializeField] private GameObject _goal;

    private IBuildingInteraction _iFace;
    private NavMeshAgent _nav;
    private BuildingManager _buildingManager;

    private delegate IEnumerator DestinationChecker();
    private delegate IEnumerator ResourceChecker();
    private DestinationChecker _destinationCheckerFunc;
    private ResourceChecker _resourceCheckerFunc;

    private Coroutine _destinationCheckerCoroutine;
    private Coroutine _resourceCheckerCoroutine;

    private WaitForSeconds _destinationCheckDelay = new WaitForSeconds(DESTINATION_CHECK_DELAY);

    // private Queue<UnitActivity> _taskQueue;

    private void Awake()
    {   
        //Subscribing to UnitManager events
        UnitManager _um = FindObjectOfType<UnitManager>();
        _um.UpdateFatigueEvent += OnUpdateFatigue;
        _um.UpdateThirstEvent += OnUpdateThirst;

        _destinationCheckerFunc = DestinationCheck;
        _resourceCheckerFunc = ResourceCheck;
    }

    void Start()
    {
        _buildingManager = FindObjectOfType<BuildingManager>();
        InitUnit();
        UnitData.UnitBehaviour.InitManagers();
        CurrentActivity = GetNextTask();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    CurrentActivity = GetNextTask();
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_iFace == null)// <<= Safety, not to interact with any colliders if the unit has no target
        {
            return;
        }

        if (other == _iFace.GetInteractionCollider())
        {
            if (_iFace.GetBuildingType() == UnitData.UnitBehaviour.ResourceBuilding)
            {
                CurrentActivity = UnitData.UnitBehaviour.CollectingActivity;
                StartCoroutine(UnitData.UnitBehaviour.CollectingResource(OnResourceCollectingDone));
                ToggleVisibility(false);
            }
            else if (_iFace.GetBuildingType() == UnitData.UnitBehaviour.DropOffBuilding ||
                     _iFace.GetBuildingType() == BuildingType.CASTLE)
            {

                CurrentActivity = UnitData.UnitBehaviour.UnloadingActivity;
                StartCoroutine(UnitData.UnitBehaviour.UnloadResource(OnResourceUnloadingDone));
            }
            _nav.isStopped = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == _iFace.GetInteractionCollider())
        {
            _iFace = null;
        }
    }

    private void OnDestroy()
    {
        UnitManager _um = FindObjectOfType<UnitManager>();
        if (_um != null)
        {
            _um.UpdateFatigueEvent -= OnUpdateFatigue;
            _um.UpdateThirstEvent -= OnUpdateThirst;
        }
    }

    public void ResetUnitActivities()
    {
        UnitData.UnitBehaviour.InitManagers();
        CurrentActivity = UnitActivity.IDLE;
        _goal = null;
        _iFace = null;
        _nav.destination = transform.position;
        Resources = 0;
        CurrentActivity = GetNextTask();
    }

    private GameObject SearchClosestBuildingOfType(BuildingType type)
    {
        return _buildingManager.GetClosestBuilding(type, transform.position).GetAwaiter().GetResult();
    }

    private void InitUnit()
    {
        _nav = GetComponent<NavMeshAgent>();
        _nav.speed = UnitData.Speed;
        CurrentActivity = UnitActivity.IDLE;
        Energy = 100;
        Water = 100;
        Resources = 0;
    }

    private void OnUpdateThirst()
    {
        // If at a well > Gain water
        // Else > Reduce water
        // Some logic if the worker is below a certain water level
    }

    private void OnUpdateFatigue()
    {
        // If in house > Gain energy
        // Else > Reduce energy
        // Some logic if the worker is below a certain energy level
    }

    private void OnResourceCollectingDone(int value)
    {
        ToggleVisibility(true);
        Resources = value;
        _nav.isStopped = false;
        CurrentActivity = GetNextTask();
    }

    private void OnResourceUnloadingDone()
    {
        UnitData.UnitBehaviour.AddResourceToStockpile(Resources);
        Resources = 0;
        _nav.isStopped = false;
        CurrentActivity = GetNextTask();
    }

    private void ToggleVisibility(bool value)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = value;
    }

    private UnitActivity GetNextTask()
    {
        if (Energy < FATIGUE_THRESHOLD)
        {
            return UnitActivity.GOING_TO_REST;
        }

        if (Water < THIRST_THRESHOLD)
        {
            return UnitActivity.GOING_TO_DRINK;
        }

        if (Resources == 0)
        {
            //If farmer, check if any of the fields need plowing

            SetDestination();
            return UnitData.UnitBehaviour.MovingToResourceActivity;
        }
        else
        {
            SetDestination();
            return UnitData.UnitBehaviour.MovingToDropOffActivity;
        }
    }

    private void SetDestination()
    {
        if (Resources > 0)
        {
            GameObject nearestCastle = SearchClosestBuildingOfType(BuildingType.CASTLE);
            GameObject nearestDropOff = SearchClosestBuildingOfType(UnitData.UnitBehaviour.DropOffBuilding);

            float toCastle = float.MaxValue;
            float toTarget = float.MaxValue;

            if (nearestCastle != null)
            {
                toCastle = Vector3.Distance(nearestCastle.transform.position, gameObject.transform.position);
            }

            if (nearestDropOff != null)
            {
                toTarget = Vector3.Distance(nearestDropOff.transform.position, gameObject.transform.position);
            }
            
            if (toCastle < toTarget)
            {
                _goal = nearestCastle;
            }
            else
            {
                _goal = nearestDropOff;
            }
        }
        else
        {
            _goal = SearchClosestBuildingOfType(UnitData.UnitBehaviour.ResourceBuilding);

        }

        if (_goal != null)
        {
            _iFace = _goal.GetComponent<IBuildingInteraction>();

            if (!_nav.SetDestination(_iFace.GetInteractionDestination()))
            {
                NavMesh.SamplePosition(_iFace.GetInteractionDestination(), out NavMeshHit hit, MESH_SEARCH_AREA, NavMesh.AllAreas);
                _nav.SetDestination(hit.position);
            }

            _nav.isStopped = false;

            StartCheckingDestination();
        }
        else
        {
            CurrentActivity = UnitActivity.IDLE;
            StartCheckingForAvailableResources();
        }

        
    }

    private void StartCheckingDestination()
    {
        if (_destinationCheckerCoroutine != null)
        {
            StopCoroutine(_destinationCheckerCoroutine);
        }

        _destinationCheckerCoroutine = StartCoroutine(_destinationCheckerFunc());
    }

    private void StartCheckingForAvailableResources()
    {
        if (_resourceCheckerCoroutine != null)
        {
            StopCoroutine(_resourceCheckerCoroutine);
        }

        _resourceCheckerCoroutine = StartCoroutine(_resourceCheckerFunc());
    }

    private IEnumerator DestinationCheck()
    {
        while (true)
        {
            yield return _destinationCheckDelay;

            if (_goal == null)
            {
                break;
            }
        }

        StartCoroutine(NewDestinationAtEndOfFrame());
        yield break;
    }

    private IEnumerator NewDestinationAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        SetDestination();
        yield break;
    }

    private IEnumerator ResourceCheck()
    {
        while (true)
        {
            yield return _destinationCheckDelay;

            if (_buildingManager.CheckAvailableBuildingsOfType(UnitData.UnitBehaviour.ResourceBuilding))
            {
                break;
            }
        }

        StartCoroutine(NewDestinationAtEndOfFrame());
        yield break;
    }
}
