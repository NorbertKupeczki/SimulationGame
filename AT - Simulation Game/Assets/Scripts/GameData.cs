using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public enum UnitType
    {
        WORKER = 0,
        FARMER = 1,
        MINER = 2,
        LUMBERJACK = 3
    }

    public enum BuildingType
    {
        ARCHERY_RANGE = 0,
        BARRACKS = 1,
        CASTLE = 2,
        HOUSE = 3,
        LUMBER_MILL = 4,
        FARM = 5,
        MARKET = 6,
        MILL = 7,
        WELL = 8,
        FOREST = 9,
        MINE = 10
    }

}

public interface IBuildingInteraction
{
    Vector3 GetInteractionDestination();
    
    Collider GetInteractionCollider();
}
