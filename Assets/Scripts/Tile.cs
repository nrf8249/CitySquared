using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Sprite[] _icons;
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _numberOverlay;

    private bool _isOccupied;
    private TileType _currentType;
    private int _currentUpgradeLevel;
    private int _prevHappiness = 0;
    private int _currentHappiness = 0;

    public int Happiness{
        get { return _currentHappiness; }
    }
    public TileType TileType{
        get { return _currentType; }
    }
    public int Level{
        get { return _currentUpgradeLevel; }
    }
    public GameObject Highlight
    {
        get { return _highlight; }
    }

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
        _currentType = TileType.Empty;
        _currentUpgradeLevel = 0;
        _currentHappiness = 0;
        _numberOverlay.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.Instance.State != GameState.GameRunning) return;

        if (TutorialManager.Instance != null && TutorialManager.Instance.isTutorialActive)
        {
            if (!TutorialManager.Instance.CanClickTile(transform.position))
            {
                return;
            }
        }
        else { _numberOverlay.SetActive(true); }
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
        else { _numberOverlay.SetActive(false); }

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
        if (GameManager.Instance.State == GameState.GameOver) return;

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
        switch (type)
        {
            case TileType.Empty:
                _renderer.color = _baseColor;
                _isOccupied = false;
                _currentType = TileType.Empty;
                break;
            case TileType.Residential:
                _renderer.color = Color.green;
                _isOccupied = true;
                _currentType = TileType.Residential;
                break;
            case TileType.Commercial:
                _renderer.color = Color.blue;
                _isOccupied = true;
                _currentType = TileType.Commercial;
                break;
            case TileType.Industrial:
                _renderer.color = Color.red;
                _isOccupied = true;
                _currentType = TileType.Industrial;
                break;
        }
        _renderer.sprite = _icons[(int)_currentType];
        OnTileChanged((int)transform.position.x, (int)transform.position.y);
    }

    public void OnTileChanged(int x, int y)
    {
        // Update this tile
        UpdateHappiness();

        // Also update neighbors since THEY are affected
        for (int i = (-1 * this.Level); i < this.Level + 1; i++)
        {
            for (int j = (-1 * this.Level); j < this.Level + 1; j++)
            {
                int nx = x + i;
                int ny = y + j;

                if (GridManager.Instance.InBounds(nx, ny))
                {
                    Tile neighbor = GridManager.Instance.GetTileAtPosition(new Vector2(nx, ny));
                    if (neighbor != null)
                    {
                        neighbor.UpdateHappiness();
                    }
                }
            }
        }
    }

    public void UpgradeTile(int current, int increment)
    {
        int newLevel = current + increment;
        if (newLevel > 3) newLevel = 3; // Cap the upgrade level at 3
        _currentUpgradeLevel = newLevel;
    }

    // Instance method: updates THIS tile's happiness and adjusts economy using this tile's delta
    public void UpdateHappiness()
    {
        _prevHappiness = _currentHappiness;

        int x = (int)transform.position.x;
        int y = (int)transform.position.y;

        int happiness = 0;

        for (int i = (-1 * this.Level); i < this.Level + 1; i++)
        {
            for (int j = (-1 * this.Level); j < this.Level + 1; j++)
            {
                int nx = x + i;
                int ny = y + j;

                if (nx < 0 || ny < 0 || nx >= GridManager.Instance.GetGridWidth() || ny >= GridManager.Instance.GetGridHeight())
                {
                    continue;
                }

                Tile neighbor = GridManager.Instance.GetTileAtPosition(new Vector2(nx, ny));
                neighbor.Highlight.SetActive(true);
                if (neighbor == null) continue;

                if (_currentType == TileType.Residential)
                {
                    if (neighbor.TileType == TileType.Residential)
                    {
                        happiness += 1; // each adjacent residential up to any cap can be handled later
                    }
                    else if (neighbor.TileType == TileType.Commercial)
                    {
                        happiness += 1;
                    }
                    else if (neighbor.TileType == TileType.Industrial)
                    {
                        happiness -= 1;
                    }
                }
                else if (_currentType == TileType.Commercial)
                {
                    if (neighbor.TileType == TileType.Residential)
                    {
                        happiness += 1;
                    }
                    else if (neighbor.TileType == TileType.Industrial)
                    {
                        happiness += 1;
                    }
                }
                else if (_currentType == TileType.Industrial)
                {
                    if (neighbor.TileType == TileType.Residential)
                    {
                        happiness += 1;
                    }
                    else if (neighbor.TileType == TileType.Commercial)
                    {
                        happiness += 1;
                    }
                }
                neighbor.Highlight.SetActive(false);
            }
        }

        // Apply any caps/limits here (example caps from your prior logic)
        // residential: max +5 from adjacent residential, +3 from adjacent commercial, industrial penalties as required
        // If you need exact caps, compute counts separately and clamp accordingly.

        // Update display and internal state
        var happinessText = transform.Find("Happiness")?.GetComponent<TextMeshPro>();
        if (happinessText != null)
        {
            happinessText.text = happiness.ToString();
        }

        _currentHappiness = happiness;

        // Adjust economy based on delta for THIS tile only
        int delta = _currentHappiness - _prevHappiness;
        if (delta != 0)
        {
            GameManager.Instance.ChangeCoins(delta);
        }
    }
}

public enum TileType
{
    Empty,
    Residential,
    Commercial,
    Industrial
}