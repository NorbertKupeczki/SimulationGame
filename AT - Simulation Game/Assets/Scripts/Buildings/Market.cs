using System.Collections;
using TMPro.Examples;
using UnityEngine;
using static GameData;

public class Market : MonoBehaviour, IBuildingInteraction, ISelectable
{
    [SerializeField] private InteractionPoint _iPoint;
    [SerializeField] private BuildingSO _buildingData;
    [SerializeField] private GameObject _buttonsPanel;

    private WaitForSeconds _goldGenerationDelay = new WaitForSeconds(GOLD_GENERATION_DELAY);

    private ResourceManager _rm;
    private UI _ui;

    private void Awake()
    {
        _iPoint = GetComponentInChildren<InteractionPoint>();
    }

    private void Start()
    {
        _buttonsPanel = FindObjectOfType<BuildingsButtonManager>().GetPanelOfBuildingType(_buildingData.buildingType);
        _rm = FindObjectOfType<ResourceManager>();
        _ui = FindObjectOfType<UI>();

        StartCoroutine(GeneratingGold());
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

    private IEnumerator GeneratingGold()
    {
        while(true)
        {
            yield return _goldGenerationDelay;
            _ui.StartWorldFloatingText(transform.position, "5 Gold");
            _rm.GainCoins(GOLD_PER_TICK);
        }
    }    
}