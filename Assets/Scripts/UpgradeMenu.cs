using UnityEngine;
using TMPro;

public class UpgradeMenu : HoverMenu
{
    public void InitializeUpgradeMenu()
    {
        // Implement logic to initialize the upgrade menu based on the selected tile's current state
        Tile selectedTile = GridManager.Instance.GetSelectedTile();
        
        //gameObject.transform.Find("TileType").GetComponent<TextMeshPro>().text = $"Tile Type: {selectedTile.GetTileType()}";
    }

    public void OnUpgradeButtonClicked()
    {
        // Implement upgrade logic here
        Debug.Log("Upgrade button clicked!");
        Tile selectedTile = GridManager.Instance.GetSelectedTile();
        //selectedTile.UpgradeTile(); 
        Destroy(gameObject);
    }
}
