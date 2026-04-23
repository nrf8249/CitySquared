using UnityEngine;
using TMPro;

public class UpgradeMenu : HoverMenu
{
    public void InitializeUpgradeMenu()
    {
        // Implement logic to initialize the upgrade menu based on the selected tile's current state
        Tile selectedTile = GridManager.Instance.GetSelectedTile();

        gameObject.transform.Find("TileType").gameObject.GetComponent<TextMeshProUGUI>().text = $"Tile Type: {selectedTile.TileType}";
        gameObject.transform.Find("UpgradeLevel").gameObject.GetComponent<TextMeshProUGUI>().text = $"Upgrade Level: {selectedTile.Level}";
        gameObject.transform.Find("Happiness").gameObject.GetComponent<TextMeshProUGUI>().text = $"Happiness: {selectedTile.Happiness}";

    }

    public void OnUpgradeButtonClicked()
    {
        // Implement upgrade logic here
        Debug.Log("Upgrade button clicked!");
        Tile selectedTile = GridManager.Instance.GetSelectedTile();
        if (GameManager.Instance.GetCoins < 10){
            // Not enough coins to upgrade the tile
            Debug.Log("Not enough coins to upgrade the tile!");
            return;
        }
        else
            GameManager.Instance.ChangeCoins(-10); // Example cost for upgrading a tile
        selectedTile.UpgradeTile(selectedTile.Level, 1); // Example: increment by 1

        Destroy(gameObject);
    }
}
