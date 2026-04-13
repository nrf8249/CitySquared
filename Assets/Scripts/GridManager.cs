using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private int _width, _height;
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Transform _cam;
    private Dictionary<Vector2, Tile> _tiles;
    private Tile selectedTile;

    void Awake()
    {
        Instance = this;
    }

    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector2(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = ((x % 2 == 0 & y % 2 != 0) || (x % 2 != 0 & y % 2 == 0));
                spawnedTile.Init(isOffset);

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float)_width/2 - 0.5f, (float)_height / 2 - 0.5f, -10);
        GameManager.Instance.ChangeState(GameState.GameRunning);
    }

    public Tile GetSelectedTile()
    {
        if (selectedTile != null)
        {
            return selectedTile;
        }
        else
        {
            Debug.LogError("No tile selected!");
            return null;
        }
    }

    public void SetSelectedTile(Tile tile)
    {
        selectedTile = tile;
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out Tile tile))
        {
            return tile;
        }
        return null;
    }

    public int GetGridWidth()
    {
        return _width;
    }  

    public int GetGridHeight()
    {
        return _height;
    }

    public bool InBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < _width && y < _height;
    }
}
