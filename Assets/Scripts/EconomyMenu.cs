using UnityEngine;

public class EconomyMenu : MonoBehaviour
{
    private TMPro.TextMeshProUGUI tmpro;

    void Awake()
    {
        tmpro = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void UpdateEconomy()
    {
        // Placeholder for economy update logic
        // This could involve updating UI elements, recalculating costs, or other economy-related tasks

        tmpro.text = $"Coins: {GameManager.Instance.GetCoins}"; // Example of updating a UI element with the current coin count
    }
}
