using UnityEngine;
using UnityEngine.UI;

public class CreationMenu : HoverMenu
{
    public Button residentialBtn;
    public Button commercialBtn;

    void Start()
    {
        if (TutorialManager.Instance != null && TutorialManager.Instance.isTutorialActive)
        {
            residentialBtn.gameObject.SetActive(false);
            commercialBtn.gameObject.SetActive(false);
        }
    }

    public void OnResidentialButtonClicked()
    {
        // Implement building logic here
        Tile selectedTile = GridManager.Instance.GetSelectedTile();
        selectedTile.SetTileType(TileType.Residential);
        if (GameManager.Instance.GetCoins < 4){
            // Not enough coins to build a residential tile
            Debug.Log("Not enough coins to build a residential tile!");
            return;
        }else
            GameManager.Instance.ChangeCoins(-4); // Example cost for building a residential tile
        Destroy(gameObject);
    }

    public void OnCommercialButtonClicked()
    {
        // Implement building logic here
        Tile selectedTile = GridManager.Instance.GetSelectedTile();
        selectedTile.SetTileType(TileType.Commercial);
        if (GameManager.Instance.GetCoins < 7){
            // Not enough coins to build a commercial tile
            Debug.Log("Not enough coins to build a commercial tile!");
            return;
            }
        else
            GameManager.Instance.ChangeCoins(-7); // Example cost for building a commercial tile
        Destroy(gameObject);
    }

    public void OnIndustrialButtonClicked()
    {
        // Implement building logic here
        Tile selectedTile = GridManager.Instance.GetSelectedTile();
        selectedTile.SetTileType(TileType.Industrial);
        if (GameManager.Instance.GetCoins < 5){
            // Not enough coins to build an industrial tile
            Debug.Log("Not enough coins to build an industrial tile!");
            return;
            }
        else
            GameManager.Instance.ChangeCoins(-5); // Example cost for building an industrial tile

        if (TutorialManager.Instance != null)
        {
            TutorialManager.Instance.OnBuildingCreated("Industrial");
        }

        Destroy(gameObject);
    }
}
