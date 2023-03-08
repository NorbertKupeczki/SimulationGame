using UnityEngine;
using static GameData;

public class MarketButtons : MonoBehaviour
{
    private ResourceManager _rm;
    private UI _ui;

    private void Start()
    {
        _rm = FindObjectOfType<ResourceManager>();
        _ui = FindObjectOfType<UI>();
    }

    public void SellWood()
    {
        if (_rm.SpendWood(TRADE_BASE_AMOUNT))
        {
            _rm.GainCoins((int)(TRADE_BASE_AMOUNT * TRADE_MULTIPLIER));
        }
        else
        {
            _ui.StartFloatText("Don't have enough wood!");
        }
    }

    public void BuyWood()
    {
        if (_rm.SpendCoins(TRADE_BASE_AMOUNT))
        {
            _rm.GainWood((int)(TRADE_BASE_AMOUNT * TRADE_MULTIPLIER));
        }
        else
        {
            _ui.StartFloatText("Don't have enough gold!");
        }
    }

    public void SellOre()
    {
        if (_rm.SpendOre(TRADE_BASE_AMOUNT))
        {
            _rm.GainCoins((int)(TRADE_BASE_AMOUNT * TRADE_MULTIPLIER));
        }
        else
        {
            _ui.StartFloatText("Don't have enough ore!");
        }
    }

    public void BuyOre()
    {
        if (_rm.SpendCoins(TRADE_BASE_AMOUNT))
        {
            _rm.GainOre((int)(TRADE_BASE_AMOUNT * TRADE_MULTIPLIER));
        }
        else
        {
            _ui.StartFloatText("Don't have enough gold!");
        }
    }

    public void SellWheat()
    {
        if (_rm.SpendWheat(TRADE_BASE_AMOUNT))
        {
            _rm.GainCoins((int)(TRADE_BASE_AMOUNT * TRADE_MULTIPLIER));
        }
        else
        {
            _ui.StartFloatText("Don't have enough wheat!");
        }
    }

    public void BuyWheat()
    {
        if (_rm.SpendCoins(TRADE_BASE_AMOUNT))
        {
            _rm.GainWheat((int)(TRADE_BASE_AMOUNT * TRADE_MULTIPLIER));
        }
        else
        {
            _ui.StartFloatText("Don't have enough gold!");
        }
    }
}
