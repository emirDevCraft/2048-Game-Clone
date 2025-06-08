using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Board : MonoBehaviour
{
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private TileStatesSO[] _tileStates;

    [SerializeField] private float _animateDuration = 0.1f;

    private List<Tile> tiles = new List<Tile>();

    private TileGrid _grid;
    private bool _isWaiting;

    private void Awake()
    {
        _grid = GetComponentInChildren<TileGrid>();
    }

    private void Start()
    {
        CreateTile();
        CreateTile();
    }

    private void Update()
    {
        if (!_isWaiting)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                MoveTiles(Vector2Int.up, 0, 1, 1, 1);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                MoveTiles(Vector2Int.down, 0, 1, _grid.GetHeight() - 2, -1);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                MoveTiles(Vector2Int.left, 1, 1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                MoveTiles(Vector2Int.right, _grid.GetWidth() - 2, -1, 0, 1);
            }
        }

    }

    public void ClearBoard() 
    {
        foreach (Cell cell in _grid.cells)
        {
            cell.tile = null;
        }

        foreach (Tile tile in tiles)
        {
            Destroy(tile.gameObject);
        }

        tiles.Clear();
    }

    public void CreateTile()
    {
        Tile tile = Instantiate(_tilePrefab, _grid.transform);
        tile.SetStates(_tileStates[0], Consts.Numbers.NUMBER_2);
        tile.SpawnTile(_grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }
    private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool isChanged = false;

        for (int x = startX; x >= 0 && x < _grid.GetWidth(); x += incrementX)
        {
            for (int y = startY; y >= 0 && y < _grid.GetHeight(); y += incrementY)
            {
                Cell cell = _grid.GetCell(x, y);

                if (cell.isOccupied)
                {
                    isChanged |= MoveTile(cell.tile, direction);
                }
            }
        }

        if (isChanged)
        {
            StartCoroutine(WaitForChanges());
        }
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        Cell newcell = null;
        Cell adjacentCell = _grid.GetAdjacentCell(tile.cell, direction);

        while (adjacentCell != null)
        {
            if (adjacentCell.isOccupied)
            {
                if (CanMerge(tile, adjacentCell.tile))
                {
                    Merge(tile, adjacentCell.tile);

                    return true;
                }

                break;
            }

            newcell = adjacentCell;
            adjacentCell = _grid.GetAdjacentCell(adjacentCell, direction);
        }

        if (newcell != null)
        {
            tile.MoveTo(newcell);
            return true;
        }

        return false;
    }

    private void Merge(Tile a, Tile b)
    {
        tiles.Remove(a);
        a.Merge(b.cell);

        int index = Mathf.Clamp(IndexOf(b.tileState) + 1, 0, _tileStates.Length - 1);
        int number = b.number * 2;

        b.SetStates(_tileStates[index], number);
        AnimateTiles(b, _animateDuration);

        GameManager.Instance.IncreaseScore(number);

        AudioManager.Instance.Play(Consts.Audio.MERGE_SOUND);
    }

    private void AnimateTiles(Tile tileToAnimate, float animateDuraiton)
    {
        tileToAnimate.gameObject.transform.DOScale(1.25f, animateDuraiton).OnComplete(() =>
        {
            tileToAnimate.gameObject.transform.DOScale(1f, animateDuraiton);
        });
    }

    private bool CanMerge(Tile a, Tile b)
    {
        return a.number == b.number && !b.isLocked;
    }



    private int IndexOf(TileStatesSO tileState)
    {
        for (int i = 0; i < _tileStates.Length; i++)
        {
            if (tileState == _tileStates[i])
            {
                return i;
            }
        }

        return -1;
    }

    private IEnumerator WaitForChanges()
    {
        _isWaiting = true;

        float waitingDuration = 0.1f;
        yield return new WaitForSeconds(waitingDuration);
        _isWaiting = false;

        foreach (Tile tile in tiles)
        {
            tile.isLocked = false;
        }

        if (tiles.Count != _grid.GetSize())
        {
            CreateTile();
        }

        if (CheckForGameOver())
        {
            GameManager.Instance.GameOver();
        }
    }

    private bool CheckForGameOver()
    {
        if (tiles.Count != _grid.GetSize())
        {
            return false;
        }

        foreach (Tile tile in tiles)
        {
            Cell upCell = _grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            Cell downCell = _grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            Cell leftCell = _grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            Cell rightCell = _grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            if (upCell != null && CanMerge(tile, upCell.tile))
            {
                return false;
            }

            else if (downCell != null && CanMerge(tile, downCell.tile))
            {
                return false;
            }

            else if (leftCell != null && CanMerge(tile, leftCell.tile))
            {
                return false;
            }

            else if (rightCell != null && CanMerge(tile, rightCell.tile))
            {
                return false;
            }

        }
        return true;
    }

}

