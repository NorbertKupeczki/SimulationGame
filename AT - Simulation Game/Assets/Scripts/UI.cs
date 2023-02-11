using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] GraphicRaycaster _graphicRaycaster;
    [SerializeField] EventSystem _eventSystem;

    private void Awake()
    {
        _graphicRaycaster = GetComponent<GraphicRaycaster>();
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
}
