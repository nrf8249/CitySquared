using UnityEngine;
using UnityEngine.UI;

public class RulesManager : MonoBehaviour
{
    public static RulesManager Instance;

    public GameObject rulesPanel;
    public Button rulesButton;
    public Button closeButton;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rulesPanel.SetActive(false);
        rulesButton.gameObject.SetActive(false);
        rulesButton.onClick.AddListener(ShowRules);
        closeButton.onClick.AddListener(HideRules);
    }

    public void ShowRulesButton()
    {
        rulesButton.gameObject.SetActive(true);
    }

    public void ShowRules()
    {
        rulesPanel.SetActive(true);
    }

    public void HideRules()
    {
        rulesPanel.SetActive(false);
    }
}