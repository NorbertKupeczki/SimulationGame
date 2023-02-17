using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [Header ("Unit info")]
    [SerializeField] private UnitSO _unitData;
    [field:SerializeField] public bool _isBusy { get; private set; }
    [field:SerializeField] public float _energy { get; private set; }
    [field:SerializeField] public float _water { get; private set; }

    [Header ("Interactions")]
    [SerializeField] private GameObject _goal;

    private IBuildingInteraction _iFace;
    private NavMeshAgent _nav;

    private void Awake()
    {
        _isBusy = false;
        _energy = 100;
        _water = 100;

        _nav = GetComponent<NavMeshAgent>();
        _nav.speed = _unitData.Speed;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject _goal = SearchClosestBuildingOfType(GameData.BuildingType.HOUSE);
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

    private GameObject SearchClosestBuildingOfType(GameData.BuildingType type)
    {
        return FindObjectOfType<BuildingManager>().GetClosestBuilding(GameData.BuildingType.HOUSE, transform.position).GetAwaiter().GetResult();
    }
}
