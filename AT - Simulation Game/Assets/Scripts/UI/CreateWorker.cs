using UnityEngine;
using static GameData;

public class CreateWorker : MonoBehaviour
{
    [SerializeField] private SelectionMarker _selection;
    [SerializeField] private UnitManager _unitManager;

    private ResourceManager _rm;
    private UI _ui;

    private void Start()
    {
        _selection = FindObjectOfType<SelectionMarker>();
        _unitManager = FindObjectOfType<UnitManager>();
        _rm = FindObjectOfType<ResourceManager>();
        _ui = FindObjectOfType<UI>();
    }

    public void CreateWorkerUnit()
    {
        if(_rm.SpendWheat(WORKER_COST))
        {
            _unitManager.CreateWorker?.Invoke(_selection.GetInteractionPointTransform());
        }
        else
        {
            _ui.StartFloatText("You need " + WORKER_COST + " wheat!");
        }
    }

    public void CreateLumberjack()
    {
        _unitManager.PromoteUnit?.Invoke(_selection.GetInteractionPointTransform(), UnitType.LUMBERJACK);
    }

    public void CreateMiner()
    {
        _unitManager.PromoteUnit?.Invoke(_selection.GetInteractionPointTransform(), UnitType.MINER);
    }

    public void CreateFarmer()
    {
        _unitManager.PromoteUnit?.Invoke(_selection.GetInteractionPointTransform(), UnitType.FARMER);
    }

    public void DemoteLumberjack()
    {
        _unitManager.DemoteUnit?.Invoke(_selection.GetSelectedBuildingTransform(), UnitType.LUMBERJACK);
    }

    public void DemoteMiner()
    {
        _unitManager.DemoteUnit?.Invoke(_selection.GetSelectedBuildingTransform(), UnitType.MINER);
    }

    public void DemoteFarmer()
    {
        _unitManager.DemoteUnit?.Invoke(_selection.GetSelectedBuildingTransform(), UnitType.FARMER);
    }
}
