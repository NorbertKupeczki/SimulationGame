using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] GameObject _camera;
    [SerializeField] float _cameraSpeed;
    [SerializeField] float _orbitSpeed;
    [SerializeField] float _zoomSpeed;
    [SerializeField] Vector2 _minMaxZoom;
    [SerializeField] GameObject _particleSystem;
    
    private Vector3 _newPosition;
    private Quaternion _newRotation;
    private Vector3 _newZoom;
    private float _speedUp;
    private const float LERP_CONST = 5.0f;
    private const float NORMAL_SPEED = 1.0f;
    private const float BOOSTED_SPEED = 2.5f;

    private InputActions _inputs;


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

        _inputs.Player.MouseClick.performed += ctx => MouseClick(ctx);
    }

    private void OnEnable()
    {
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }

    private void Update()
    {
        MoveCamera(_inputs.Player.Move.ReadValue<Vector2>());
        OrbitCamera(_inputs.Player.Orbit.ReadValue<float>());
        ZoomCamera(_inputs.Player.Zoom.ReadValue<float>());
    }

    public void MoveCamera(Vector2 vec)
    {
        if (vec != Vector2.zero)
        {
            Vector3 movement = (transform.forward * vec.y + transform.right * vec.x) * _cameraSpeed * _speedUp;
            _newPosition = transform.position + movement;
        }
        transform.position = Vector3.Lerp(transform.position, _newPosition, LERP_CONST * Time.deltaTime);
    }

    public void OrbitCamera(float direction)
    {
        if(Mathf.Abs(direction) > 0)
        {
            _newRotation *= Quaternion.AngleAxis(direction * _orbitSpeed * _speedUp, Vector3.up);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, LERP_CONST * Time.deltaTime );
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
        Debug.Log("Test button pressed" + context.ReadValueAsButton());
    }

    private void MouseClick(InputAction.CallbackContext context)
    {
        Ray ray = _camera.GetComponent<Camera>().ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            Debug.Log(raycastHit.collider.gameObject);
            Instantiate(_particleSystem, raycastHit.point, Quaternion.identity);
        }
    }

    public void ZoomCamera(float direction)
    {
        if (Mathf.Abs(direction) > 0)
        {
            float curZoom = _camera.transform.position.y;
            float deltaZoom = -direction * _zoomSpeed;

            if (curZoom + deltaZoom < _minMaxZoom.x)
            {
                _newZoom = SetZoom(_minMaxZoom.x);
            }
            else if (curZoom + deltaZoom > _minMaxZoom.y)
            {
                _newZoom = SetZoom(_minMaxZoom.y);
            }
            else
            {
                _newZoom = SetZoom(curZoom + deltaZoom);
            }
        }        
        _camera.transform.localPosition = Vector3.Lerp(_camera.transform.localPosition, _newZoom, LERP_CONST * Time.deltaTime);
    }

    private Vector3 SetZoom(float value)
    {
        return new Vector3(0.0f, value, -value);
    }
}
