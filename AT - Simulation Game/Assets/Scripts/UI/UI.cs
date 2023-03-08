using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] GraphicRaycaster _graphicRaycaster;
    [SerializeField] EventSystem _eventSystem;
    [SerializeField] TextMeshProUGUI _tooltipText;
    [SerializeField] FloatingText _floatingText;
    [SerializeField] FloatingWorldText _floatingWorldText;
    [SerializeField] Camera _camera;

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
        FloatingWorldText newText = Instantiate(_floatingWorldText, gameObject.transform);
        newText.InitText(_camera, position, text);
    }
}
