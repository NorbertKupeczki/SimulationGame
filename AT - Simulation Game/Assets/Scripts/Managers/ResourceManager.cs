using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    [Header("Starting Resources")]
    [SerializeField] private int _startingCoins;
    [SerializeField] private int _startingWood;
    [SerializeField] private int _startingOre;
    [SerializeField] private int _startingWheat;

    [Header("Text fields")]
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI _woodText;
    [SerializeField] private TextMeshProUGUI _oreText;
    [SerializeField] private TextMeshProUGUI _wheatText;

    [Header("Building buttons")]
    [SerializeField] private List<GameObject> _buildingButtons;

    private int _coins;
    private int _wood;
    private int _ore;
    private int _wheat;

    private Coroutine _buttonUpdateRoutine;

    private void Awake()
    {
        InitStartingResources();
        _buttonUpdateRoutine = StartCoroutine(UpdateButtonStates());
    }

    private void InitStartingResources()
    {
        _coins = _startingCoins;
        _wood = _startingWood;
        _ore = _startingOre;
        _wheat = _startingWheat;

        UpdateUI();
    }

    #region Check resource functions
    /// <summary>
    /// Checks that the player has a certain amount of coins.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>bool</returns>     
    public bool HasEnoughCoins(int value)
    {
        return _coins >= value;
    }

    /// <summary>
    /// Checks that the player has a certain amount of wood.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>bool</returns>    
    public bool HasEnoughWood(int value)
    {
        return _wood >= value;
    }

    /// <summary>
    /// Checks that the player has a certain amount of ore.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>bool</returns>    
    public bool HasEnoughOre(int value)
    {
        return _ore >= value;
    }

    /// <summary>
    /// Checks that the player has a certain amount of wheat.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>bool</returns>    
    public bool HasEnoughWheat(int value)
    {
        return _wheat >= value;
    }
    #endregion

    #region Gain resource functions
    /// <summary>
    /// Adds the parameter to the available coins.
    /// </summary>
    /// <param name="value"></param>
    public void GainCoins(int value)
    {
        _coins += value;
        UpdateUI(_coinText, _coins);
    }

    /// <summary>
    /// Adds the parameter to the available wood.
    /// </summary>
    /// <param name="value"></param>
    public void GainWood(int value)
    {
        _wood += value;
        UpdateUI(_woodText, _wood);
    }

    /// <summary>
    /// Adds the parameter to the available ore.
    /// </summary>
    /// <param name="value"></param>
    public void GainOre(int value)
    {
        _ore += value;
        UpdateUI(_oreText, _ore);
    }

    /// <summary>
    /// Adds the parameter to the available wheat.
    /// </summary>
    /// <param name="value"></param>
    public void GainWheat(int value)
    {
        _wheat += value;
        UpdateUI(_wheatText, _wheat);
    }
    #endregion

    #region Spend resource functions
    /// <summary>
    /// Reduces the amount of coins by the parameter, if there is enough.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>True if successful, false if not</returns>
    public bool SpendCoins(int value)
    {
        if (HasEnoughCoins(value))
        {
            _coins -= value;
            UpdateUI(_coinText, _coins);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Reduces the amount of wood by the parameter, if there is enough.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>True if successful, false if not</returns>
    public bool SpendWood(int value)
    {
        if (HasEnoughWood(value))
        {
            _wood -= value;
            UpdateUI(_woodText, _wood);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Reduces the amount of ore by the parameter, if there is enough.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>True if successful, false if not</returns>
    public bool SpendOre(int value)
    {
        if (HasEnoughOre(value))
        {
            _ore -= value;
            UpdateUI(_oreText, _ore);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Reduces the amount of wheat by the parameter, if there is enough.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>True if successful, false if not</returns>
    public bool SpendWheat(int value)
    {
        if (HasEnoughWheat(value))
        {
            _wheat -= value;
            UpdateUI(_wheatText, _wheat);
            return true;
        }
        return false;
    }
    #endregion

    public bool SpendResources(BuildingSO buildingData)
    {
        if (AffordabilityCheck(buildingData))
        {
            SpendCoins(buildingData._coinCost);
            SpendWood(buildingData._woodCost);
            SpendOre(buildingData._oreCost);
            SpendWheat(buildingData._wheatCost);
            return true;
        }
        return false;
    }

    public bool AffordabilityCheck(BuildingSO buildingData)
    {
        if (!HasEnoughCoins(buildingData._coinCost)) return false;
        else if (!HasEnoughWood(buildingData._woodCost)) return false;
        else if (!HasEnoughOre(buildingData._oreCost)) return false;
        else if (!HasEnoughWheat(buildingData._wheatCost)) return false;
        return true;
    }

    private IEnumerator UpdateButtonStates()
    {
        while(true)
        {
            foreach(GameObject button in _buildingButtons)
            {
                Button buttonScript = button.GetComponent<Button>();
                bool isAffordable = AffordabilityCheck(buttonScript.GetBuildingData());
                
                if (isAffordable && !buttonScript.IsButtonInteractable())
                {
                    button.GetComponent<Button>().SwithcButton(true);
                }
                else if (!isAffordable && buttonScript.IsButtonInteractable())
                {
                    button.GetComponent<Button>().SwithcButton(false);
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }

    private void UpdateUI()
    {
        _coinText.text = _coins.ToString();
        _woodText.text = _wood.ToString();
        _oreText.text = _ore.ToString();
        _wheatText.text = _wheat.ToString();
    }

    private void UpdateUI(TextMeshProUGUI toUpdate, int value)
    {
        toUpdate.text = value.ToString();
    }

}
