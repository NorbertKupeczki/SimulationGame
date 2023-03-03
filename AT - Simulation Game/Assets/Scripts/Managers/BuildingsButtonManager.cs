using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameData;

public class BuildingsButtonManager : MonoBehaviour
{
    [SerializeField] List<GameObject> _buildingButtonPanels;

    private Dictionary<BuildingType, int> _panelLists = new Dictionary<BuildingType, int>
    {
        { GameData.BuildingType.ARCHERY_RANGE, 0 },
        { GameData.BuildingType.BARRACKS, 1 },
        { GameData.BuildingType.CASTLE, 2 },
        { GameData.BuildingType.HOUSE, 3 },
        { GameData.BuildingType.LUMBER_MILL, 4 },
        { GameData.BuildingType.MARKET, 5 },
        { GameData.BuildingType.MILL, 6 },
        { GameData.BuildingType.WELL, 7 },
        { GameData.BuildingType.FARM, 8 },
        { GameData.BuildingType.FOREST, 9 },
        { GameData.BuildingType.MINE, 10 },
    };

    void Start()
    {
        foreach (GameObject panel in _buildingButtonPanels)
        {
            panel.SetActive(false);
        }
    }

    public GameObject GetPanelOfBuildingType(BuildingType type)
    {
        return _buildingButtonPanels[_panelLists[type]];
    }
}
