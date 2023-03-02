using UnityEngine;
using static GameData;

public class CreateWorker : MonoBehaviour
{
    [SerializeField] private SelectionMarker _selection;
    [SerializeField] private UnitManager _unitManager;

    private void Start()
    {
        _selection = FindObjectOfType<SelectionMarker>();
        _unitManager = FindObjectOfType<UnitManager>();
    }

    public void CreateWorkerUnit()
    {
        _unitManager.CreateWorker?.Invoke(_selection.GetInteractionPointTransform());
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
