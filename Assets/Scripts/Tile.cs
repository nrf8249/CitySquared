using UnityEngine;
using UnityEngine.EventSystems;

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
        _currentHappiness = GridManager.Instance.CalculateTileHappiness(); // Update happiness based on the new tile type
    }

    public TileType GetTileType()
    {
        return _currentType;
    }

    public void UpgradeTile(int level)
    {
        switch (level)
        {
            case 1: break;
            case 2: break;
            case 3: break;
            default: Debug.LogWarning("Invalid upgrade level"); break;
        }
    }

    public int GetUpgradeLevel()
    {
        return _currentUpgradeLevel;
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