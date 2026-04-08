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
    private UpgradeLevel _currentUpgradeLevel;

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
        _isOccupied = false;
        _occupyingBuilding = null;
        _currentType = TileType.Empty;
        _currentUpgradeLevel = UpgradeLevel.Level1;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _highlight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) 
    { 
        _highlight.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!_isOccupied)
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
                break;
            case TileType.Residential:
                _renderer.color = Color.green; // Example: Set to green for residential
                _isOccupied = true; // Mark as occupied when set to residential
                break;
            case TileType.Commercial:
                _renderer.color = Color.blue; // Example: Set to blue for commercial
                _isOccupied = true; // Mark as occupied when set to commercial
                break;
            case TileType.Industrial:
                _renderer.color = Color.orange; // Example: Set to gray for industrial
                _isOccupied = true; // Mark as occupied when set to industrial
                break;
        }
    }

    public TileType GetTileType()
    {
        return _currentType;
    }

    public void UpgradeTile(UpgradeLevel level)
    {
        switch (level)
        {
            case UpgradeLevel.Level1:
                // Implement logic for Level 1 upgrade
                break;
            case UpgradeLevel.Level2:
                // Implement logic for Level 2 upgrade
                break;
            case UpgradeLevel.Level3:
                // Implement logic for Level 3 upgrade
                break;
            default:
                Debug.LogWarning("Invalid upgrade level");
                break;
        }
    }

    public UpgradeLevel GetUpgradeLevel()
    {
        return _currentUpgradeLevel;
    }
}

public enum TileType
{
    Empty,
    Residential,
    Commercial,
    Industrial
}

public enum UpgradeLevel
{
    Level1,
    Level2,
    Level3
}
