using UnityEngine;
using UnityEngine.EventSystems;


public class MenuManager : MonoBehaviour
{   
    public static MenuManager Instance;
    [SerializeField] private GameObject creationMenuPrefab;
    [SerializeField] private GameObject upgradeMenuPrefab;
    [SerializeField] private Canvas canvas;

    void Awake()
    {
        Instance = this;
    }   

    public void ShowCreationMenu(Vector2 position)
    {
        GameObject creationMenu = Instantiate(creationMenuPrefab, position, Quaternion.identity);
        creationMenu.transform.SetParent(canvas.transform, false); // Set the canvas as the parent of the menu
        Vector2 screenPos = Camera.main.WorldToScreenPoint(position); // Convert world position to screen position

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            screenPos,
            null,
            out Vector2 localPoint); // Convert screen position to local position in the canvas

        RectTransform rt = creationMenu.GetComponent<RectTransform>();
        rt.anchoredPosition = localPoint; // Set the anchored position of the menu to the local point

    }

    //needs updating to match the creation menu, but for now we can just reuse the same position
    public void ShowUpgradeMenu(Vector2 position)
    {
        var upgradeMenu = Instantiate(upgradeMenuPrefab, position, Quaternion.identity);
        upgradeMenu.transform.SetParent(canvas.transform, false); // Set the canvas as the parent of the menu
    }
}
