using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [Header("Tutorial UI Elements")]
    public GameObject tutorialCanvas;
    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;
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

    public void StartTutorial()
    {
        isTutorialActive = true;
        tutorialStep = 1;
        tutorialCanvas.SetActive(true);
        dialogBox.SetActive(true);
        maskImage.SetActive(true);

        dialogText.text = "Welcome to the city!\n" +
            "As the mayor, you need to zone areas on the grid.\n" +
            "Now, click the highlighted tile to construct your first building.";

        Tile targetTile = GridManager.Instance.GetTileAtPosition(targetTilePos);
        if (targetTile != null)
        {
            targetTile.ForceHighlight(true);
            targetTile.SetSortingOrder(20);
        }
        else
        {
            Debug.LogError("error");
        }
    }

    public void OnTargetTileClicked()
    {
        if (tutorialStep == 1)
        {
            tutorialStep = 2;
            dialogText.text = "Good job! Now, build an Industrial zone here.";

        }
    }

    public bool CanClickTile(Vector2 tilePos)
    {
        if (!isTutorialActive) return true;

        if (tutorialStep == 1 && tilePos == targetTilePos) return true;

        return false;
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

            dialogText.text = "Congratulations! You've successfully built the city's very first building!\n" +
                              "\n" +
                              "A building's Happiness is determined by the 8 tiles immediately surrounding it:\n" +
                              "- Residential: Wants 5 Residential, 3 Commercial. " +
                              "1-2 adjacent Industrial deducts 1 pt; more deducts 2 pts.\n" +
                              "- Commercial: Wants 6 Residential, 2 Industrial.\n" +
                              "- Industrial: Wants 3 Residential, 5 Commercial.\n\n" +
                              "Plan your layout carefully to maximize your score! " +
                              "Click anywhere on the screen to begin your mayoral journey!";
        }
    }

    public void EndTutorial()
    {
        if (tutorialStep == 3)
        {
            isTutorialActive = false;
            tutorialCanvas.SetActive(false);
        }
    }

    void Update()
    {
        if (tutorialStep == 3 && Input.GetMouseButtonDown(0))
        {
            EndTutorial();
        }
    }
}