using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    [SerializeField] private Image _iconBackground;
    [SerializeField] private BuildingSO _buildingData;
    [SerializeField] private TextMeshProUGUI _tooltipText;
    [SerializeField] private CursorManager _cursorManager;
    [SerializeField] private UnityEngine.UI.Button _buttonComp;
    [SerializeField] private Tooltips _tooltips;

    private void Awake()
    {
        _cursorManager = FindObjectOfType<CursorManager>();
        _tooltips = FindObjectOfType<Tooltips>();

        _iconBackground.sprite = _buildingData._buildingIcon;
    }

    public void OnPointerEnter()
    {
        _tooltips.ShowTooltips(_buildingData);
    }

    public void OnPointerExit()
    {
        _tooltips.HideTooltips();
    }

    public void SelectBuilding()
    {
        _cursorManager.BuildingButtonClicked(_buildingData);
    }

    public BuildingSO GetBuildingData()
    {
        return _buildingData;
    }

    public void SwithcButton(bool value)
    {
        _buttonComp.interactable = value;
    }

    public bool IsButtonInteractable()
    {
        return _buttonComp.interactable;
    }
}
