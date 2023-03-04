using UnityEngine;

public static class GameData
{
    public const float MESH_SEARCH_AREA = 1.5f;

    public const float THIRST_THRESHOLD = 30.0f;
    public const float FATIGUE_THRESHOLD = 30.0f;

    public const float WATER_LOSS_MIN = 1.8f;
    public const float WATER_LOSS_MAX = 3.4f;

    public const float ENERGY_LOSS_MIN = 0.6f;
    public const float ENERGY_LOSS_MAX = 1.4f;

    public const float WATER_GAIN = 40.0f;
    public const float ENERGY_GAIN = 10.0f;

    public const float SPEED_PENALTY = 0.25f;

    public const float WOODCUTTING_TIME = 8.0f;
    public const float MINING_TIME = 10.0f;
    public const float HARVESTING_TIME = 2.0f;
    public const float UNLOADING_TIME = 1.0f;

    public const float SOWING_WHEAT_TIME = 3.0f;

    public const int WOOD_YIELD = 10;
    public const int ORE_YIELD = 10;
    public const int WHEAT_YIELD = 10;

    public const float REFUND_FACTOR = 0.5f;

    public const float DESTINATION_CHECK_DELAY = 0.2f;

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

    public enum UnitActivity
    {
        IDLE = 0,
        GOING_TO_REST = 1,
        RESTING = 2,
        GOING_TO_DRINK = 3,
        DRINKING = 4,
        GOING_TO_FOREST = 5,
        CUTTING_WOOD = 6,
        GOING_TO_MINE = 7,
        MINING_ORE = 8,
        GOING_TO_FARM = 9,
        HARVESTING_WHEAT = 10,
        SOWING_WHEAT = 11,
        GOING_TO_LUMBERMILL = 12,
        GOING_TO_BLACKSMITH = 13,
        GOING_TO_MILL = 14,
        DELIVERING_GOODS = 15
    }
}

public interface IBuildingInteraction
{
    Vector3 GetInteractionDestination();
    
    Collider GetInteractionCollider();

    GameData.BuildingType GetBuildingType();

    bool IsAvailable();

    void InteractWithBuilding();
}

public interface ISelectable
{
    void IsSelected();

    void IsDeselected();

    Transform GetTransform();

    Transform GetInteractionPointTransform();

    void DestroyBuilding();

    BuildingSO GetBuildingData();
}

