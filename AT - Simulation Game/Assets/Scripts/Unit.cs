using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using static GameData;

public class Unit : MonoBehaviour
{
    [field: Header ("Unit info")]
    [field:SerializeField] public UnitSO UnitData { get; set; }
    [field:SerializeField] public bool IsBusy { get; private set; }
    [field:SerializeField] public float Energy { get; private set; }
    [field:SerializeField] public float Water { get; private set; }

    [Header ("Interactions")]
    [SerializeField] private GameObject _goal;

    private IBuildingInteraction _iFace;
    private NavMeshAgent _nav;

    private void Awake()
    {
        IsBusy = false;
        Energy = 100;
        Water = 100;

        _nav = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        InitUnit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject _goal = SearchClosestBuildingOfType(UnitData.BuildingType);
            _iFace = _goal.GetComponent<IBuildingInteraction>();      

            if (!_nav.SetDestination(_iFace.GetInteractionDestination()))
            {
                NavMesh.SamplePosition(_iFace.GetInteractionDestination(), out NavMeshHit hit, 1.2f, NavMesh.AllAreas);
                _nav.SetDestination(hit.position);
            }

            _nav.isStopped = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_iFace == null)// <<= Safety, not to interact with any colliders if the unit has no target
        {
            return;
        }

        if (other == _iFace.GetInteractionCollider())
        {
            Debug.Log("Destination reached");
            _nav.isStopped = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<InteractionPoint>())
        {
            _iFace = null;
        }
    }

    private GameObject SearchClosestBuildingOfType(BuildingType type)
    {
        return FindObjectOfType<BuildingManager>().GetClosestBuilding(type, transform.position).GetAwaiter().GetResult();
    }

    private void InitUnit()
    {
        _nav.speed = UnitData.Speed;
    }
}
