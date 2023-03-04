using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameData;

public class WheatResource : MonoBehaviour, IBuildingInteraction, ISelectable
{
    [Header ("Base data")]
    [SerializeField] private GameObject _resource;
    [SerializeField] [Range (0.0f,100.0f)] private float _growth;
    [SerializeField] WheatState _state;
    [SerializeField] BuildingSO _buildingData;
    [SerializeField] private GameObject _buttonsPanel;

    private InteractionPoint _iPoint;
    private const float MAX_GROWTH = 100.0f;
    private const float GROWTH_TIME = 10.0f; // <<= Set to somehigher value for the final game

    private Vector3 _ripePosition;
    private Vector3 _harvestedPosition;

    private Coroutine _growingWheat_CR;

    private GameObject _farmer;

    public enum WheatState
    {
        HARVESTED = 0,
        GROWING = 1,
        RIPE = 2
    }

    private void Awake()
    {
        Vector3 position = _resource.transform.localPosition;
        _ripePosition = position;
        _harvestedPosition = new Vector3(position.x, -_resource.GetComponent<Renderer>().bounds.extents.y * 2.0f - 0.01f, position.z);

        _resource.transform.localPosition = _harvestedPosition;
        _state = WheatState.HARVESTED;

        _growingWheat_CR = StartCoroutine(GrowingWheat());
        _iPoint = GetComponentInChildren<InteractionPoint>();
    }
    
    void Start()
    {
        _buttonsPanel = FindObjectOfType<BuildingsButtonManager>().GetPanelOfBuildingType(_buildingData.buildingType);
    }

    void Update()
    {

    }

    private void OnDisable()
    {
        StopCoroutine(_growingWheat_CR);
    }

    public void RegisterFarmer(GameObject unit)
    {
        _farmer = unit;
    }

    public void UnregisterFarmer()
    {
        _farmer = null;
    }

    public bool HasFarmer()
    {
        return _farmer != null;
    }

    public bool IsHarvestable()
    {
        return _state == WheatState.RIPE;
    }

    public bool IsHarvested()
    {
        return _state == WheatState.HARVESTED;
    }

    private void Harvest()
    {
        if (_state == WheatState.RIPE)
        {
            _growth -= WHEAT_YIELD;

            if (_growth == 0)
            {
                _resource.transform.localPosition = _harvestedPosition;
                _state = WheatState.HARVESTED;
                _growth = 0.0f;
            }            
        }
    }

    private void SowWheat()
    {
        if (_state == WheatState.HARVESTED)
        {
            _state = WheatState.GROWING;
        }
    }

    private IEnumerator GrowingWheat()
    {
        while(true)
        {
            yield return new WaitUntil(() => _state == WheatState.GROWING);
            while(_growth < MAX_GROWTH)
            {
                float deltaGrowth = MAX_GROWTH / GROWTH_TIME * Time.deltaTime;

                if (Mathf.Abs(MAX_GROWTH - _growth) < deltaGrowth)
                {
                    _growth = MAX_GROWTH;
                    _resource.transform.localPosition = _ripePosition;
                    _state = WheatState.RIPE;
                }
                else
                {
                    _growth += deltaGrowth;
                    _resource.transform.localPosition = Vector3.Lerp(_harvestedPosition, _ripePosition, _growth / MAX_GROWTH);
                }
                yield return null;
            }            
        }
    }

    public Collider GetInteractionCollider()
    {
        return _iPoint.GetComponent<Collider>();
    }

    public Vector3 GetInteractionDestination()
    {
        return _iPoint.transform.position;
    }

    public BuildingType GetBuildingType()
    {
        return _buildingData.buildingType;
    }

    public bool IsAvailable()
    {
        return _state == WheatState.RIPE;
    }

    public void InteractWithBuilding()
    {
        if (IsHarvestable())
        {
            Harvest();
        }
        else if (IsHarvested())
        {
            SowWheat();
        }
    }

    public void IsSelected()
    {
        _buttonsPanel.SetActive(true);
    }

    public void IsDeselected()
    {
        _buttonsPanel.SetActive(false);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public Transform GetInteractionPointTransform()
    {
        return _iPoint.transform;
    }

    public void DestroyBuilding()
    {
        FindObjectOfType<BuildingManager>().RemoveBuilding(gameObject);
        Destroy(gameObject);
    }

    public BuildingSO GetBuildingData()
    {
        return _buildingData;
    }
}
