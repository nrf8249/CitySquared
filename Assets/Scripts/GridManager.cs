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
        Debug.Log($"Selected tile at position: {tile.transform.position}");
    }

    public int GetTileHappiness()
    {
        int happiness = 0;
        int numAdjacentResidential = 0;
        int numAdjacentIndustrial = 0;
        int numAdjacentCommercial = 0;
        if (selectedTile != null)
        {
            // Implement logic to calculate happiness based on the selected tile's type and upgrades
            for (float x = selectedTile.transform.position.x - 1; x <= selectedTile.transform.position.x + 2; x++)
            {
                for (float y = selectedTile.transform.position.y - 1; y <= selectedTile.transform.position.y + 2; y++)
                {
                    if (_tiles.TryGetValue(new Vector2(x, y), out Tile adjacentTile))
                    {
                        if (selectedTile.GetComponent<Tile>().GetTileType() == TileType.Residential)
                        {
                            //+1 as long as <= 5 adjacent residential, -1 as long as only 1 adjacent industrial, -2 for >= 1, +1 if adjacent to commercial
                            if (adjacentTile.GetComponent<Tile>().GetTileType() == TileType.Residential)
                            {
                                numAdjacentResidential++;
                                if (numAdjacentResidential <= 5) happiness += 1;
                            }
                            if (adjacentTile.GetComponent<Tile>().GetTileType() == TileType.Industrial)
                            {
                                numAdjacentIndustrial++;
                                if (numAdjacentIndustrial == 1) happiness -= 1;
                                else if (numAdjacentIndustrial > 1) happiness -= 2;
                            }
                            if (adjacentTile.GetComponent<Tile>().GetTileType() == TileType.Commercial)
                            {
                                numAdjacentCommercial++;
                                if (numAdjacentCommercial <= 3) happiness += 1;
                            }
                        }
                        else if (selectedTile.GetComponent<Tile>().GetTileType() == TileType.Industrial)
                        {

                            if (adjacentTile.GetComponent<Tile>().GetTileType() == TileType.Residential)
                            {
                                numAdjacentResidential++;
                                if (numAdjacentResidential <= 3) happiness += 1;
                            }
                            if (adjacentTile.GetComponent<Tile>().GetTileType() == TileType.Industrial)
                            {
                                numAdjacentIndustrial++;
                            }
                            if (adjacentTile.GetComponent<Tile>().GetTileType() == TileType.Commercial)
                            {
                                numAdjacentCommercial++;
                                if (numAdjacentCommercial <= 5) happiness += 1;
                            }
                        }
                        else if (selectedTile.GetComponent<Tile>().GetTileType() == TileType.Commercial)
                        {
                            if (adjacentTile.GetComponent<Tile>().GetTileType() == TileType.Residential)
                            {
                                numAdjacentResidential++;
                                if (numAdjacentResidential <= 6) happiness += 1;
                            }
                            if (adjacentTile.GetComponent<Tile>().GetTileType() == TileType.Industrial)
                            {
                                numAdjacentIndustrial++;
                                if (numAdjacentIndustrial <= 2) happiness += 1;
                            }
                            if (adjacentTile.GetComponent<Tile>().GetTileType() == TileType.Commercial)
                            {
                                numAdjacentCommercial++;
                            }
                        }
                    }

                }
                
            }
            return happiness; // Placeholder return value
        }
        else
        {
            Debug.LogError("No tile selected!");
            return 0; // Return a default value when no tile is selected
        }
    }
}
