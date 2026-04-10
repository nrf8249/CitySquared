using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [Header("Tutorial UI Elements")]
    public GameObject tutorialCanvas;
    public GameObject dialogBox;
    public GameObject welcomeText;
    public GameObject hintText1;
    public GameObject hintText2;
    public GameObject rulesText;
    public GameObject endText;
    public GameObject maskImage;

    [Header("Tutorial State")]
    public bool isTutorialActive = false;
    public int tutorialStep = 0;

    public Vector2 targetTilePos = new Vector2(2, 2);

    void Awake()
    {
        Instance = this;

        if (tutorialCanvas != null)
        {
            tutorialCanvas.SetActive(false);
        }
    }

    private void HideAllTexts()
    {
        welcomeText.SetActive(false);
        hintText1.SetActive(false);
        hintText2.SetActive(false);
        rulesText.SetActive(false);
        endText.SetActive(false);
    }

    public void StartTutorial()
    {
        isTutorialActive = true;
        tutorialStep = 0;
        tutorialCanvas.SetActive(true);
        dialogBox.SetActive(true);
        maskImage.SetActive(true);

        HideAllTexts();
        welcomeText.SetActive(true);
    }

    void Update()
    {
        if (!isTutorialActive) return;

        if (tutorialStep == 0 && Input.GetMouseButtonDown(0))
        {
            tutorialStep = 1;
            HideAllTexts();
            hintText1.SetActive(true);

            Tile targetTile = GridManager.Instance.GetTileAtPosition(targetTilePos);
            if (targetTile != null)
            {
                targetTile.ForceHighlight(true);
                targetTile.SetSortingOrder(20);
            }
        }
        else if (tutorialStep == 3 && Input.GetMouseButtonDown(0))
        {
            tutorialStep = 4;
            HideAllTexts();
            endText.SetActive(true);
        }
        else if (tutorialStep == 4 && Input.GetMouseButtonDown(0))
        {
            EndTutorial();
        }
    }

    public void OnTargetTileClicked()
    {
        if (tutorialStep == 1)
        {
            tutorialStep = 2;
            HideAllTexts();
            hintText2.SetActive(true);
        }
    }

    public void OnBuildingCreated(string buildingType)
    {
        if (tutorialStep == 2 && buildingType == "Industrial")
        {
            tutorialStep = 3;

            Tile targetTile = GridManager.Instance.GetTileAtPosition(targetTilePos);
            if (targetTile != null)
            {
                targetTile.ForceHighlight(false);
                targetTile.SetSortingOrder(0);
            }

            HideAllTexts();
            rulesText.SetActive(true);
        }
    }

    public bool CanClickTile(Vector2 tilePos)
    {
        if (!isTutorialActive) return true;
        if (tutorialStep == 1 && tilePos == targetTilePos) return true;
        return false;
    }

    public void EndTutorial()
    {
        isTutorialActive = false;
        tutorialCanvas.SetActive(false);
        RulesManager.Instance.ShowRulesButton();
    }
}