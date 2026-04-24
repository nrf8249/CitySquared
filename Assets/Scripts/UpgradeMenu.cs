using UnityEngine;
using TMPro;

public class UpgradeMenu : HoverMenu
{
    public void InitializeUpgradeMenu()
    {
        // Implement logic to initialize the upgrade menu based on the selected tile's current state
        Tile selectedTile = GridManager.Instance.GetSelectedTile();

        gameObject.transform.Find("TileType").gameObject.GetComponent<TextMeshProUGUI>().text = $"Tile Type: {selectedTile.GetTileType()}";
        gameObject.transform.Find("UpgradeLevel").gameObject.GetComponent<TextMeshProUGUI>().text = $"Upgrade Level: {selectedTile.GetUpgradeLevel()}";
        gameObject.transform.Find(" Happiness").gameObject.GetComponent<TextMeshProUGUI>().text = $"Happiness: {selectedTile.GetHappiness()}";

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
