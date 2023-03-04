using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static GameData;

public class Mill : MonoBehaviour, IBuildingInteraction, ISelectable
{
    [SerializeField] private GameObject _blades;
    [SerializeField][Range(0.0f, 80.0f)] private float _rotationSpeed = 0.0f;
    [SerializeField][Range(0.5f, 0.9f)] private float _lerpSpeed = 0.7f;
    [SerializeField] private bool _bladesRotationOn = false;
    [SerializeField] BuildingSO _buildingData;
    [SerializeField] private GameObject _buttonsPanel;

    private const float _MAX_ROTATION_SPEED = 80.0f;
    private const float _ROTATION_TOLERANCE = 1.0f;

    private InteractionPoint _iPoint;

    private void Awake()
    {
        _iPoint = GetComponentInChildren<InteractionPoint>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _buttonsPanel = FindObjectOfType<BuildingsButtonManager>().GetPanelOfBuildingType(_buildingData.buildingType);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBlades();
    }

    public void ToggleMill()
    {
        _bladesRotationOn = !_bladesRotationOn;
    }

    private void UpdateBlades()
    {
        if (_bladesRotationOn && _rotationSpeed < _MAX_ROTATION_SPEED)
        {
            if (Mathf.Abs(_MAX_ROTATION_SPEED - _rotationSpeed) < _ROTATION_TOLERANCE)
            {
                _rotationSpeed = _MAX_ROTATION_SPEED;
            }
            else
            {
                _rotationSpeed = Mathf.Lerp(_rotationSpeed, _MAX_ROTATION_SPEED, _lerpSpeed * Time.deltaTime);            
            }
        }
        else if (!_bladesRotationOn && _rotationSpeed > 0.0f)
        {
            if (_rotationSpeed < _ROTATION_TOLERANCE)
            {
                _rotationSpeed = 0.0f;
            }
            else
            {
                _rotationSpeed = Mathf.Lerp(_rotationSpeed, 0.0f, _lerpSpeed * Time.deltaTime);
            }
        }

        if(_rotationSpeed > 0.0f)
        {
            _blades.transform.Rotate(_blades.transform.forward, _rotationSpeed * Time.deltaTime, Space.World);
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
        return true;
    }

    public void InteractWithBuilding()
    {

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
