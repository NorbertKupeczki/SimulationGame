using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] GraphicRaycaster _graphicRaycaster;
    [SerializeField] EventSystem _eventSystem;
    [SerializeField] TextMeshProUGUI _tooltipText;
    [SerializeField] FloatingText _insufficientResourcesPrefab;

    private void Awake()
    {
        _graphicRaycaster = GetComponent<GraphicRaycaster>();
        _tooltipText.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        FloatingText newText = Instantiate(_insufficientResourcesPrefab, gameObject.transform);
        newText.SetText(text);
    }
}
