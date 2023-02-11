using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private int _coins;
    private int _wood;
    private int _ore;
    private int _wheat;

    private void Awake()
    {
        InitStartingResources();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitStartingResources()
    {
        _coinText.text = _startingCoins.ToString();
        _woodText.text = _startingWood.ToString();
        _oreText.text = _startingOre.ToString();
        _wheatText.text = _startingWheat.ToString();

        _coins = _startingCoins;
        _wood = _startingWood;
        _ore = _startingOre;
        _wheat = _startingWheat;
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
    }

    /// <summary>
    /// Adds the parameter to the available wood.
    /// </summary>
    /// <param name="value"></param>
    public void GainWood(int value)
    {
        _wood += value;
    }

    /// <summary>
    /// Adds the parameter to the available ore.
    /// </summary>
    /// <param name="value"></param>
    public void GainOre(int value)
    {
        _ore += value;
    }

    /// <summary>
    /// Adds the parameter to the available wheat.
    /// </summary>
    /// <param name="value"></param>
    public void GainWheat(int value)
    {
        _wheat += value;
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
            return true;
        }
        return false;
    }
    #endregion
}
