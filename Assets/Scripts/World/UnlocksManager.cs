using UnityEngine;
using System.Collections;

[DefaultExecutionOrder(-600)]
public class UnlocksManager : Singleton<UnlocksManager>
{
    public InventoryItem DashItem;
    public InventoryItem DoubleJumpItem;
    public InventoryItem AirDashItem;

    private Player _player;
    private InventoryManager _inventoryManager;

    // Use this for initialization
    public void Init()
    {
        _inventoryManager = InventoryManager.Instance;
        _player = Player.Instance;

        if(_inventoryManager.HasItem(DashItem))
        {
            Player.Instance.Dash.enabled = true;
        }

        if(_inventoryManager.HasItem(DoubleJumpItem))
        {
            Player.Instance.Jump.numAirJumps = 1;
        }

        if (_inventoryManager.HasItem(AirDashItem))
        {
            Player.Instance.Dash.enabled = true;
            Player.Instance.Dash.numAirDashes = 1;
        }

        _inventoryManager.onAddItem += UnlockAbilities;
    }

    void UnlockAbilities(InventoryItem item)
    {
        if (item == DashItem)
        {
            Player.Instance.Dash.enabled = true;
        }

        if (item == DoubleJumpItem)
        {
            Player.Instance.Jump.numAirJumps = 1;
        }

        if (item == AirDashItem)
        {
            Player.Instance.Dash.enabled = true;
            Player.Instance.Dash.numAirDashes = 1;
        }
    }
}
