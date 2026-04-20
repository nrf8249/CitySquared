using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    public GameObject startMenuUI;

    private int coins = 30;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeState(GameState.MainMenu);                
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
                TutorialManager.Instance.StartTutorial();
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
        startMenuUI.SetActive(true);
    }
}

public enum GameState
{
    MainMenu = -1,
    GenerateGrid = 0,
    GameRunning = 1,
    GameOver = 2
}

