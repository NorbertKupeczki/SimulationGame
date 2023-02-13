using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D _defaultCursor;
    PointerType _pointerType;
    [SerializeField] private SelectionMarker _marker;

    private GraphicRaycaster _graphicRaycaster;
    private PointerEventData _pointerEventData;
    private EventSystem _eventSystem;

    public enum PointerType
    {
        BASIC = 0,
        PLACEMENT = 1
    }

    private void Awake()
    {
        _pointerType = PointerType.BASIC;
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
    }

    public void HandleLeftClick(Camera camera)
    {
        _pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>(5);
        _graphicRaycaster.Raycast(_pointerEventData, results);

        if (results.Count > 0)
        {
            return;
        }

        Ray ray = camera.GetComponent<Camera>().ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            //Debug.Log(raycastHit.collider.gameObject.tag);
            //Instantiate(_particleSystem, raycastHit.point, Quaternion.identity);
            if (raycastHit.collider.gameObject.CompareTag("Selectable"))
            {
                _marker.SetBuilding(raycastHit.collider.gameObject);
            }
        }
    }

    public void HandleRightClick()
    {
        _marker.CancelSelection();
    }
}
