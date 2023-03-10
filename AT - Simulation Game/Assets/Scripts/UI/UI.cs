using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [Header ("Default options")]
    [SerializeField] GraphicRaycaster _graphicRaycaster;
    [SerializeField] EventSystem _eventSystem;
    [SerializeField] TextMeshProUGUI _tooltipText;
    [Space]
    [Header ("Floating Text")]
    [SerializeField] Camera _camera;
    [SerializeField] FloatingText _floatingText;
    [SerializeField] FloatingWorldText _floatingWorldText;
    [SerializeField] Canvas _worldSpaceFloatingTextCanvas;

    private void Awake()
    {
        _graphicRaycaster = GetComponent<GraphicRaycaster>();
        _tooltipText.enabled = false;
    }

    public void TestClick()
    {
        Debug.Log("Button has been clicked");
    }

    public GraphicRaycaster GetGraphicsRaycaster()
    {
        return _graphicRaycaster;
    }

    public EventSystem GetEventSystem()
    {
        return _eventSystem;
    }

    public void StartFloatText(string text)
    {
        FloatingText newText = Instantiate(_floatingText, gameObject.transform);
        newText.SetText(text);
    }

    public void StartWorldFloatingText(Vector3 position, string text)
    {
        FloatingWorldText newText = Instantiate(_floatingWorldText, _worldSpaceFloatingTextCanvas.transform);
        newText.InitText(_camera, position, text);
    }
}
