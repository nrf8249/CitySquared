using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;

    private bool _isOccupied;
    private GameObject _occupyingBuilding;
    private TileType _currentType;
    private int _currentUpgradeLevel;
    private int _currentHappiness;

    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(0, 1), // Up
        new Vector2Int(1, 1), // Up-Right
        new Vector2Int(1, 0), // Right
        new Vector2Int(1, -1), // Down-Right
        new Vector2Int(0, -1), // Down
        new Vector2Int(-1, -1), // Down-Left
        new Vector2Int(-1, 0), // Left
        new Vector2Int(-1, 1) // Up-Left
    };

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
        _isOccupied = false;
        _occupyingBuilding = null;
        _currentType = TileType.Empty;
        _currentUpgradeLevel = 0;
        _currentHappiness = 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (TutorialManager.Instance != null && TutorialManager.Instance.isTutorialActive)
        {
            if (!TutorialManager.Instance.CanClickTile(transform.position))
            {
                return;
            }
        }
        _highlight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (TutorialManager.Instance != null &&
            TutorialManager.Instance.isTutorialActive &&
            TutorialManager.Instance.tutorialStep == 1 &&
            (Vector2)transform.position == TutorialManager.Instance.targetTilePos)
        {
            return;
        }

        _highlight.SetActive(false);
    }

    public void ForceHighlight(bool isOn)
    {
        if (_highlight != null)
        {
            _highlight.SetActive(isOn);
        }
    }

    public void SetSortingOrder(int order)
    {
        _renderer.sortingOrder = order;
        if (_highlight != null)
        {
            SpriteRenderer highlightRenderer = _highlight.GetComponent<SpriteRenderer>();
            if (highlightRenderer != null)
            {
                highlightRenderer.sortingOrder = order + 1; 
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (TutorialManager.Instance != null && TutorialManager.Instance.isTutorialActive)
        {
            if (!TutorialManager.Instance.CanClickTile(transform.position))
            {
                return;
            }

            TutorialManager.Instance.OnTargetTileClicked();
        }

        if (!_isOccupied)
        {
            GridManager.Instance.SetSelectedTile(this);
            MenuManager.Instance.ShowCreationMenu(transform.position);
        }
        else
        {
            GridManager.Instance.SetSelectedTile(this);
            MenuManager.Instance.ShowUpgradeMenu(transform.position);
        }
    }

    public void SetTileType(TileType type)
    {
        // Implement logic to change the tile's appearance and functionality based on the type
        switch (type)
        {
            case TileType.Empty:
                _renderer.color = _baseColor; // Example: Set to base color for empty
                _isOccupied = false; // Mark as not occupied when set to empty
                _currentType = TileType.Empty; // Update current type
                break;
            case TileType.Residential:
                _renderer.color = Color.green; // Example: Set to green for residential
                _isOccupied = true; // Mark as occupied when set to residential
                _currentType = TileType.Residential; // Update current type
                break;
            case TileType.Commercial:
                _renderer.color = Color.blue; // Example: Set to blue for commercial
                _isOccupied = true; // Mark as occupied when set to commercial
                _currentType = TileType.Commercial; // Update current type
                break;
            case TileType.Industrial:
                _renderer.color = Color.red; // 
                _isOccupied = true; // Mark as occupied when set to industrial
                _currentType = TileType.Industrial; // Update current type
                break;
        }
        OnTileChanged((int)transform.position.x, (int)transform.position.y);
    }

    public void OnTileChanged(int x, int y)
    {
        Debug.Log($"Tile at ({x}, {y}) changed to {_currentType}");
        UpdateHappiness(x, y);
        UpgradeTile(_currentUpgradeLevel, 1);

        // Also update neighbors since THEY are affected
        foreach (var dir in directions)
        {
            int nx = x + dir.x;
            int ny = y + dir.y;

            if (GridManager.Instance.InBounds(nx, ny))
            {
                UpdateHappiness(nx, ny);
            }
        }
    }

    public TileType GetTileType()
    {
        return _currentType;
    }

    public void UpgradeTile(int current, int increment)
    {
        int newLevel = current + increment;
        if (newLevel > 3) newLevel = 3; // Cap the upgrade level at 3
    }

    public int GetUpgradeLevel()
    {
        return _currentUpgradeLevel;
    }

    public void UpdateHappiness(int x, int y)
    {
        Debug.Log($"Updating happiness for tile at ({x}, {y}) of type {_currentType}");
        Tile current = GridManager.Instance.GetTileAtPosition(new Vector2(x, y));
        int happiness = 0;

        foreach (var dir in directions)
        {
            int nx = x + dir.x;
            int ny = y + dir.y;

            if (nx < 0 || ny < 0 || nx >= GridManager.Instance.GetGridWidth() || ny >= GridManager.Instance.GetGridHeight())
            {
                continue;
            }

            Tile neighbor = GridManager.Instance.GetTileAtPosition(new Vector2(nx, ny));
            int numAdjacentResidential = 0;
            int numAdjacentIndustrial = 0;
            int numAdjacentCommercial = 0;
            if (neighbor != null)
            {
                if (current.GetTileType() == TileType.Residential)
                {
                    if (neighbor.GetTileType() == TileType.Residential)
                    {
                        numAdjacentResidential++;
                        if (numAdjacentResidential <= 5) happiness += 1;
                    }
                    else if (neighbor.GetTileType() == TileType.Commercial)
                    {
                        numAdjacentCommercial++;
                        if (numAdjacentCommercial <= 3) happiness += 1;
                    }
                    else if (neighbor.GetTileType() == TileType.Industrial)
                    {
                        numAdjacentIndustrial++;
                        if (numAdjacentIndustrial == 1) happiness -= 1;
                        else if (numAdjacentIndustrial > 1) happiness -= 2;
                    }
                }
                if (current.GetTileType() == TileType.Commercial)
                {
                    if (neighbor.GetTileType() == TileType.Residential)
                    {
                        numAdjacentResidential++;
                        if (numAdjacentResidential <= 6) happiness += 1;
                    }
                    else if (neighbor.GetTileType() == TileType.Commercial)
                    {
                        numAdjacentCommercial++;
                    }
                    else if (neighbor.GetTileType() == TileType.Industrial)
                    {
                        numAdjacentIndustrial++;
                        if (numAdjacentIndustrial <= 2) happiness += 1;
                    }
                }
                if (current.GetTileType() == TileType.Industrial)
                {
                    if (neighbor.GetTileType() == TileType.Residential)
                    {
                        numAdjacentResidential++;
                        if (numAdjacentResidential <= 3) happiness += 1;
                    }
                    else if (neighbor.GetTileType() == TileType.Commercial)
                    {
                        numAdjacentCommercial++;
                        if (numAdjacentCommercial <= 5) happiness += 1;
                    }
                    else if (neighbor.GetTileType() == TileType.Industrial)
                    {
                        numAdjacentIndustrial++;
                    }
                }
            }
            Debug.Log($"Calculated happiness for tile at ({x}, {y}) is {happiness} from {numAdjacentResidential} adjacent residential, {numAdjacentCommercial} adjacent commercial, {numAdjacentIndustrial} adjacent industrial");
        }
        current.transform.Find("Happiness").GetComponent<TextMeshPro>().text = happiness.ToString();
        _currentHappiness = happiness;
    }

    public int GetHappiness()
    {
        return _currentHappiness;
    }   
}

public enum TileType
{
    Empty,
    Residential,
    Commercial,
    Industrial
}