using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }

    public void ChangeState(GameState newState)
    {
        State = newState;
        switch (newState){   
            case GameState.GenerateGrid:
                GridManager.Instance.GenerateGrid();
                break;
            case GameState.GameRunning:
                // Start game logic here
                break;
            case GameState.GameOver:
                // Handle game over logic here
                break;
        }
    }
}

public enum GameState
{
    GenerateGrid = 0,
    GameRunning = 1,
    GameOver = 2
}

