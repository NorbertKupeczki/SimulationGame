using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltips : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _tooltipText;
    [SerializeField] private GameObject _coinTooltips;
    [SerializeField] private GameObject _woodTooltips;
    [SerializeField] private GameObject _oreTooltips;
    [SerializeField] private GameObject _wheatTooltips;

    private Vector3 _startPosition;
    private Vector3 _step;

    private void Awake()
    {
        _step = new Vector3(130.0f,0.0f,0.0f);
        _startPosition = _coinTooltips.transform.localPosition;
        _tooltipText = GetComponent<TextMeshProUGUI>();
        _tooltipText.text = "Tooltips";
        HideTooltips();
    }

    public void ShowTooltips(BuildingSO buildingData)
    {
        _tooltipText.text = buildingData._buildingName;
        _tooltipText.enabled = true;
        int index = 0;

        ShowIcon(_coinTooltips, buildingData._coinCost, ref index);
        ShowIcon(_woodTooltips, buildingData._woodCost, ref index);
        ShowIcon(_oreTooltips, buildingData._oreCost, ref index);
        ShowIcon(_wheatTooltips, buildingData._wheatCost, ref index);
    }

    public void HideTooltips()
    {
        _tooltipText.enabled = false;
        _coinTooltips.SetActive(false);
        _woodTooltips.transform.localPosition = _startPosition;
        _woodTooltips.SetActive(false);
        _oreTooltips.transform.localPosition = _startPosition;
        _oreTooltips.SetActive(false);
        _wheatTooltips.transform.localPosition = _startPosition;
        _wheatTooltips.SetActive(false);
    }

    private void ShowIcon(GameObject icon, int value, ref int index)
    {
        if (value > 0)
        {
            icon.SetActive(true);
            icon.transform.localPosition = _startPosition + _step * index;
            icon.GetComponentInChildren<TextMeshProUGUI>().text = value.ToString();
            index += 1;
        }
    }
}
