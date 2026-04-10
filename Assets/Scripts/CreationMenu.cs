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

        if (TutorialManager.Instance != null)
        {
            TutorialManager.Instance.OnBuildingCreated("Industrial");
        }

        Destroy(gameObject);
    }
}
