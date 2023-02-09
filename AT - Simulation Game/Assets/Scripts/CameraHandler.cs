using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] GameObject _camera;
    [SerializeField] float _cameraSpeed;
    [SerializeField] float _orbitSpeed;
    [SerializeField] float _zoomSpeed;
    [SerializeField] Vector2 _minMaxZoom;
    [SerializeField] GameObject _particleSystem;
    [SerializeField] SelectionMarker _marker;

    private Vector3 _newPosition;
    private Quaternion _newRotation;
    private Vector3 _newZoom;
    private float _speedUp;
    private const float LERP_CONST = 5.0f;
    private const float NORMAL_SPEED = 1.0f;
    private const float BOOSTED_SPEED = 2.5f;

    private InputActions _inputs;
    private bool _middleMouseKeyDown = false;
    private bool _rightMouseKeyHeld = false;

    [Header("Mouse")]
    [SerializeField] [Range(0.5f, 1.5f)] private float _dragSensitivity = 1.0f;
    [SerializeField] [Range(0.5f, 1.5f)] private float _orbitSensitivity = 1.0f;
    [SerializeField] [Range(0.5f, 1.5f)] private float _zoomSensitivity = 1.0f;
    private const float DRAG_MODIFIER = 1.1f;
    private const float ORBIT_MODIFIER = 0.2f;
    private const float ZOOM_MODIFIER = 0.15f;

    private void Awake()
    {
        _camera = gameObject.GetComponentInChildren<Camera>().gameObject;

        _newPosition = transform.position;
        _newRotation = transform.rotation;
        _newZoom = _camera.transform.localPosition;
        _speedUp = NORMAL_SPEED;

        _newPosition = transform.position;
        _inputs = new InputActions();

        _inputs.Player.Action.performed += ctx => TestButton(ctx);
        _inputs.Player.Action.canceled += ctx => TestButton(ctx);

        _inputs.Player.SpeedUp.performed += ctx => SpeedUpButton(ctx);
        _inputs.Player.SpeedUp.canceled += ctx => SpeedUpButton(ctx);

        _inputs.Player.MouseClickLeft.performed += ctx => MouseClickLeft(ctx);

        _inputs.Player.MouseClickMiddle.performed += ctx => MouseClickMiddle(ctx);
        _inputs.Player.MouseClickMiddle.canceled += ctx => MouseClickMiddle(ctx);

        _inputs.Player.MouseClickRight.performed += ctx => MouseClickRight(ctx);
        _inputs.Player.MouseClickRight.canceled += ctx => MouseClickRight(ctx);

        _inputs.Player.MouseScroll.performed += ctx => MouseScroll(ctx);

        _inputs.Player.MouseMoveDelta.performed += ctx => MouseDelta(ctx);

    }

    private void Update()
    {
        MoveCamera(_inputs.Player.Move.ReadValue<Vector2>());
        OrbitCamera(_inputs.Player.Orbit.ReadValue<float>());
        ZoomCamera(_inputs.Player.Zoom.ReadValue<float>());

        UpdateCameraFocusTransform();
    }

    private void OnEnable()
    {
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }

    #region"Keyboard events"
    public void MoveCamera(Vector2 vec)
    {
        if (vec != Vector2.zero)
        {
            Vector3 movement = (transform.forward * vec.y + transform.right * vec.x) * _cameraSpeed * _speedUp;
            _newPosition = transform.position + movement;
        }
    }

    public void OrbitCamera(float direction)
    {
        if(Mathf.Abs(direction) > 0)
        {
            _newRotation *= Quaternion.AngleAxis(direction * _orbitSpeed * _speedUp, Vector3.up);
        }        
    }

    public void ZoomCamera(float direction)
    {
        if (Mathf.Abs(direction) > 0)
        {
            float curZoom = _camera.transform.position.y;
            float deltaZoom = -direction * _zoomSpeed;

            SetNewZoom(curZoom + deltaZoom);
        }        
    }

    public void SpeedUpButton(InputAction.CallbackContext context)
    {
        if(context.ReadValueAsButton())
        {
            Debug.Log("speed up");
            _speedUp = BOOSTED_SPEED;
        }
        else
        {
            Debug.Log("speed down");
            _speedUp = NORMAL_SPEED;
        }
    }

    public void TestButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Debug.Log("Test button pressed" + context.ReadValueAsButton());
            /*
            if (Gamepad.current != null)
                Debug.Log("Gamepad Id: "+Gamepad.current.deviceId);

            if (Mouse.current != null)
                Debug.Log("Mouse Id: " + Mouse.current.deviceId);

            if (Keyboard.current != null)
                Debug.Log("Keyboard Id: " + Keyboard.current.deviceId);

            Debug.Log(context.control.path);
            Debug.Log(context.control.device.deviceId);
            */
        }
    }
    #endregion

    #region"Mouse events"
    private void MouseClickLeft(InputAction.CallbackContext context)
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = _camera.GetComponent<Camera>().ScreenPointToRay(Mouse.current.position.ReadValue());
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

    private void MouseClickMiddle(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            _middleMouseKeyDown = true;
        }
        else
        {
            _middleMouseKeyDown = false;
        }
    }

    private void MouseClickRight(InputAction.CallbackContext context)
    {
        /*
        if (context.ReadValueAsButton())
        {
            _rightMouseKeyHeld = true;
        }
        else
        {
            _rightMouseKeyHeld = false;
        }
        */

        if (context.interaction is TapInteraction && context.performed)
        {
            _marker.CancelSelection();
        }
        else if (context.interaction is HoldInteraction && context.performed)
        {
            _rightMouseKeyHeld = true;
        }
        else
        {
            _rightMouseKeyHeld = false;
        }
    }

    private void MouseScroll(InputAction.CallbackContext context)
    {
        float curZoom = _camera.transform.position.y;
        float deltaZoom = -context.ReadValue<float>() * ZOOM_MODIFIER * _zoomSensitivity;

        SetNewZoom(curZoom + deltaZoom);
    }

    private void MouseDelta(InputAction.CallbackContext context)
    {
        if (_middleMouseKeyDown)
        {
            Vector3 mouseDelta = _dragSensitivity * -DRAG_MODIFIER * (transform.forward * context.ReadValue<Vector2>().y +
                                  transform.right * context.ReadValue<Vector2>().x);
            _newPosition = transform.position + mouseDelta;
        }

        if (_rightMouseKeyHeld)
        {
            _newRotation *= Quaternion.AngleAxis(context.ReadValue<Vector2>().x * ORBIT_MODIFIER * _orbitSensitivity, Vector3.up);
        }
    }
    #endregion

    private void UpdateCameraFocusTransform()
    {
        transform.position = Vector3.Lerp(transform.position, _newPosition, LERP_CONST * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, LERP_CONST * Time.deltaTime);
        _camera.transform.localPosition = Vector3.Lerp(_camera.transform.localPosition, _newZoom, LERP_CONST * Time.deltaTime);
    }
    private void SetNewZoom(float zoom)
    {
        if (zoom < _minMaxZoom.x)
        {
            _newZoom = GetZoomVector(_minMaxZoom.x);
        }
        else if (zoom > _minMaxZoom.y)
        {
            _newZoom = GetZoomVector(_minMaxZoom.y);
        }
        else
        {
            _newZoom = GetZoomVector(zoom);
        }
    }

    private Vector3 GetZoomVector(float value)
    {
        return new Vector3(0.0f, value, -value);
    }
}
