using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CostIcons : MonoBehaviour
{
    [SerializeField] private GameObject _coinCost;
    [SerializeField] private GameObject _woodCost;
    [SerializeField] private GameObject _oreCost;
    [SerializeField] private GameObject _wheatCost;

    private List<GameObject> _icons = new List<GameObject>(4);

    private void Awake()
    {
        _icons.Add(_coinCost);
        _icons.Add(_woodCost);
        _icons.Add(_oreCost);
        _icons.Add(_wheatCost);

        SwitchCostBar(false);
    }

    /// <summary>
    /// Turn the cost bar ON/OFF
    /// </summary>
    /// <param name="value"></param>
    public void SwitchCostBar(bool value)
    {
        if (!value)
        {
            ResetCosts();
        }

        foreach(GameObject icon in _icons)
        {            
            icon.SetActive(value);            
        }
    }

    public void SetCosts(BuildingSO buildingData)
    {
        SetValue(_coinCost, buildingData._coinCost);
        SetValue(_woodCost, buildingData._woodCost);
        SetValue(_oreCost, buildingData._oreCost);
        SetValue(_wheatCost, buildingData._wheatCost);
    }

    private void ResetCosts()
    {
        SetValue(_coinCost, 0);
        SetValue(_woodCost, 0);
        SetValue(_oreCost, 0);
        SetValue(_wheatCost, 0);
    }

    private void SetValue(GameObject icon, int value)
    {
        if (value > 0)
        {
            icon.GetComponentInChildren<TextMeshProUGUI>().text = value.ToString();
            icon.SetActive(true);
        }
    }
}
