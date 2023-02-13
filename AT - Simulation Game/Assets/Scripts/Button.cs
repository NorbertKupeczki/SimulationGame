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

    private void Awake()
    {
        _iconBackground.sprite = _buildingData._buildingIcon;
    }

    public void OnPointerEnter()
    {
        _tooltipText.text = _buildingData._buildingName;
        _tooltipText.enabled = true;
    }

    public void OnPointerExit()
    {
        _tooltipText.enabled = false;
    }
}
