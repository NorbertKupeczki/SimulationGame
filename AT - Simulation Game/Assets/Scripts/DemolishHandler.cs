using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemolishHandler : MonoBehaviour
{
    [SerializeField] private GameObject _panel;

    private CursorManager _cm;
    private SelectionMarker _marker;
    private ResourceManager _rm;
    private NavMeshUpdater _nav;
    private void Awake()
    {
        _panel.SetActive(false);
    }

    void Start()
    {
        _cm = FindObjectOfType<CursorManager>();
        _marker = FindObjectOfType<SelectionMarker>();
        _rm = FindObjectOfType<ResourceManager>();
        _nav = FindObjectOfType<NavMeshUpdater>();
    }

    public void EnablePanel()
    {
        _panel.SetActive(true);
        _cm.DemolishActive();
    }

    public void ConfirmDemolition()
    {
        _rm.RefundBuildingCost(_marker.GetSelectedBuildingInterface().GetBuildingData());
        _marker.GetSelectedBuildingTransform().GetComponent<ISelectable>().DestroyBuilding();
        _marker.CancelSelection();
        _panel?.SetActive(false);
        _cm.DemolishInactive();

        _nav.RefreshNavMeshAtEndOfFrame();
    }

    public void CancelDemolition()
    {
        _panel?.SetActive(false);
        _cm.DemolishInactive();
    }
}
