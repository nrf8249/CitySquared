using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    [SerializeField] private GameObject creationMenuPrefab;
    [SerializeField] private GameObject upgradeMenuPrefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Vector2 menuOffset = new Vector2(20f, 20f);

    // Tracks the currently open menu to prevent multiple menus from stacking
    private GameObject currentMenu;

    void Awake()
    {
        Instance = this;
    }

    public void ShowCreationMenu(Vector2 position)
    {
        // Destroy the existing menu before opening a new one to prevent overlap and logic bugs
        if (currentMenu != null)
        {
            Destroy(currentMenu);
        }

        currentMenu = Instantiate(creationMenuPrefab, canvas.transform, false); // Set the canvas as the parent of the menu

        SetupMenuPosition(currentMenu);
    }

    //needs updating to match the creation menu, but for now we can just reuse the same position
    public void ShowUpgradeMenu(Vector2 position)
    {
        // Destroy the existing menu before opening a new one
        if (currentMenu != null)
        {
            Destroy(currentMenu);
        }

        currentMenu = Instantiate(upgradeMenuPrefab, canvas.transform, false); // Set the canvas as the parent of the menu

        SetupMenuPosition(currentMenu);

        currentMenu.GetComponent<UpgradeMenu>().InitializeUpgradeMenu(); // Call the initialization method to set up the upgrade menu
    }

    // Handles dynamic pivot and positioning based on mouse screen location
    private void SetupMenuPosition(GameObject menu)
    {
        RectTransform rt = menu.GetComponent<RectTransform>();
        rt.localScale = Vector3.one;

        Vector2 mouseScreenPos = Input.mousePosition;

        // Default pivot to Top-Left (0, 1) and offset to Bottom-Right
        float pivotX = 0f;
        float pivotY = 1f;
        float offsetX = menuOffset.x;
        float offsetY = -menuOffset.y; // Negative Y moves it down

        // If mouse is on the right half of the screen, flip pivot to Top-Right and offset to Left
        if (mouseScreenPos.x > Screen.width / 2f)
        {
            pivotX = 1f;
            offsetX = -menuOffset.x;
        }

        // If mouse is on the bottom half of the screen, flip pivot to Bottom and offset Upwards
        if (mouseScreenPos.y < Screen.height / 2f)
        {
            pivotY = 0f;
            offsetY = menuOffset.y; // Positive Y moves it up
        }

        // Apply calculated pivot
        rt.pivot = new Vector2(pivotX, pivotY);

        // Apply offset to prevent covering the cursor
        mouseScreenPos += new Vector2(offsetX, offsetY);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            mouseScreenPos,
            null,
            out Vector2 localPoint); // Convert screen position to local position in the canvas

        rt.anchoredPosition = localPoint;
    }

    public void UpdateEconomyMenu()
    {
        GameObject economyMenu = canvas.transform.Find("EconomyMenu")?.gameObject;
        if (economyMenu != null)
        {
            economyMenu.GetComponent<EconomyMenu>().UpdateEconomy();
        }
    }
}