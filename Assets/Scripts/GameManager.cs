using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    public GameObject startMenuUI;
    public GameObject gameOverUI;

    public static bool isRestarting = false;

    private int coins = 30;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (isRestarting)
        {
            startMenuUI.SetActive(false);
            ChangeState(GameState.GenerateGrid);
        }
        else
        {
            ChangeState(GameState.MainMenu);
        }

        MenuManager.Instance.UpdateEconomyMenu();

    }

    public void StartGame() 
    {
        startMenuUI.SetActive(false);
        ChangeState(GameState.GenerateGrid);
    }

    public void ChangeState(GameState newState)
    {
        State = newState;
        switch (newState){   
            case GameState.MainMenu:
                // Handle main menu logic here
                break;
            case GameState.GenerateGrid:                
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.GameRunning:
                // Start tutorial
                if (!isRestarting)
                {
                    TutorialManager.Instance.StartTutorial();
                }
                else
                {
                    if (RulesManager.Instance != null)
                    {
                        RulesManager.Instance.ShowRulesButton();
                    }
                }
                break;
            case GameState.GameOver:
                EndGame();
                break;
        }
    }

    public void ChangeCoins(int amount)
    {
        coins += amount;
        MenuManager.Instance.UpdateEconomyMenu();
        if (coins < 4)
        {
            ChangeState(GameState.GameOver);
        }
    }
     public int GetCoins
    {
        get { return coins; }
    }

    private void EndGame()
    {
        // Implement game over logic here, such as showing a game over screen or resetting the game
        Debug.Log("Game Over! You have run out of coins.");
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
    }
    public void RestartGame()
    {
        isRestarting = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

public enum GameState
{
    MainMenu = -1,
    GenerateGrid = 0,
    GameRunning = 1,
    GameOver = 2
}

