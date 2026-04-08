using UnityEngine;

public class CreationMenu : HoverMenu
{
    public void OnResidentialButtonClicked()
    {
        // Implement building logic here
        Debug.Log("residential button clicked!");
        Tile selectedTile = GridManager.Instance.GetSelectedTile();
        selectedTile.SetTileType(TileType.Residential);
        Destroy(gameObject);

    }

    public void OnCommercialButtonClicked()
    {
        // Implement building logic here
        Debug.Log("commercial button clicked!");
        Tile selectedTile = GridManager.Instance.GetSelectedTile();
        selectedTile.SetTileType(TileType.Commercial);
        Destroy(gameObject);

    }

    public void OnIndustrialButtonClicked()
    {
        // Implement building logic here
        Debug.Log("industrial button clicked!");
        Tile selectedTile = GridManager.Instance.GetSelectedTile();
        selectedTile.SetTileType(TileType.Industrial);
        Destroy(gameObject);


    }
}
