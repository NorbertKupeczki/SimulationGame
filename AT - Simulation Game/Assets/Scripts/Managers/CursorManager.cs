using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D _defaultCursor;
    [SerializeField] private PointerType _pointerType;
    [SerializeField] private SelectionMarker _selectionMarker;
    [SerializeField] private BuildingSO _selectedBuilding;
    [SerializeField] private PlacementMarker _placementMarker;
    [SerializeField] private Vector3 _buildPosition;

    private GraphicRaycaster _graphicRaycaster;
    private PointerEventData _pointerEventData;
    private EventSystem _eventSystem;
    private Camera _camera;
    private UI _ui;
    private BuildingManager _buildingManager;
    private CostIcons _costIcons;

    public enum PointerType
    {
        DEFAULT = 0,
        PLACEMENT = 1,
        DEMOLISH = 3
    }
    
    private void Awake()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _ui = FindObjectOfType<UI>();
        _buildingManager = FindObjectOfType<BuildingManager>();
        _costIcons = FindObjectOfType<CostIcons>();

        _selectedBuilding = null;
        _pointerType = PointerType.DEFAULT;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(_defaultCursor, Vector2.zero, CursorMode.Auto);

        UI uiScript = FindObjectOfType<UI>().GetComponent<UI>();
        _graphicRaycaster = uiScript.GetGraphicsRaycaster();
        _eventSystem = uiScript.GetEventSystem();
        _pointerEventData = new PointerEventData(_eventSystem);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDecalProjector();
    }

    public void HandleLeftClick()
    {
        _pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>(5);
        _graphicRaycaster.Raycast(_pointerEventData, results);

        if (results.Count > 0)
        {
            if (_pointerType == PointerType.PLACEMENT)
            {
                CancelBuildingPlacement();
            }
            else if (_pointerType == PointerType.DEMOLISH &&
                     CheckRaycastResults(results))
            {

                CancelDemolition();
            }
            return;
        }

        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        
        if (_pointerType == PointerType.DEFAULT &&
            Physics.Raycast(ray, out RaycastHit raycastHit, 300.0f, LayerMask.GetMask("Buildings")))
        {            
            //Debug.Log(raycastHit.collider.gameObject.tag);
            if (raycastHit.collider.gameObject.GetComponentInParent<ISelectable>() is ISelectable)
            {
                _selectionMarker.SetBuilding(raycastHit.collider.gameObject, raycastHit.collider.gameObject.GetComponentInParent<ISelectable>());
                return;
            }
            else { return; }            
        }
        else if (_pointerType == PointerType.PLACEMENT)
        {
            PlaceBuilding(_buildPosition);
        }
        else if (_pointerType == PointerType.DEMOLISH)
        {
            CancelDemolition();
        }
        
    }

    public void HandleRightClick()
    {
        _selectionMarker.CancelSelection();

        if(_pointerType == PointerType.PLACEMENT)
        {
            CancelBuildingPlacement();
        }
    }

    private void PlaceBuilding(Vector3 position)
    {
        
        bool obstacle = Physics.CheckSphere(position, 1.0f, LayerMask.GetMask("Buildings")) ||
                        Physics.CheckSphere(position, 1.0f, LayerMask.GetMask("Units"));

        if (obstacle)
        {
            _ui.StartFloatText("Can't build there!");
        }
        else if(!_buildingManager.ConstructBuilding(_selectedBuilding, position))
        {         
            _ui.StartFloatText("Insufficient resources");            
        }
        CancelBuildingPlacement();
    }

    public void BuildingButtonClicked(BuildingSO buildingData)
    {
        _selectionMarker.CancelSelection();
        _selectedBuilding = buildingData;

        _costIcons.SetCosts(buildingData);

        SwitchToPlacementMode();
    }
    
    private void CancelBuildingPlacement()
    {
        _placementMarker.SetProjector(false);
        _selectedBuilding = null;

        _costIcons.SwitchCostBar(false);

        _pointerType = PointerType.DEFAULT;
    }

    public void DemolishActive()
    {
        _pointerType = PointerType.DEMOLISH;
    }

    public void DemolishInactive()
    {
        if (_pointerType != PointerType.PLACEMENT)
        {
            _pointerType = PointerType.DEFAULT;
        }
    }

    private void CancelDemolition()
    {
        FindObjectOfType<DemolishHandler>().CancelDemolition();
        _pointerType = PointerType.DEFAULT;
    }

    private void SwitchToPlacementMode()
    {
        _placementMarker.SetProjector(true);
        if (_pointerType == PointerType.DEMOLISH)
        {
            CancelDemolition();
        }
        _pointerType = PointerType.PLACEMENT;
    }

    private void UpdateDecalProjector()
    {
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Physics.Raycast(ray, out RaycastHit raycastHit, 300.0f, LayerMask.GetMask("Ground"));
        _placementMarker.transform.position = new Vector3(raycastHit.point.x, 0.003f, raycastHit.point.z);
        _buildPosition = raycastHit.point;
    }

    private bool CheckRaycastResults(List<RaycastResult> results)
    {
        foreach(RaycastResult result in results)
        {
            if(result.gameObject.GetComponentInParent<DemolishHandler>())
            {
                return false;
            }
        }
        return true;
    }
}
