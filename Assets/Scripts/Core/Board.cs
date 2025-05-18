using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private TileStatesSO[] _tileStates;

    private List<Tile> tiles = new List<Tile>();
    private TileGrid _grid;

    private void Awake()
    {
        _grid = GetComponentInChildren<TileGrid>();
    }

    private void Start()
    {
        CreateTile();
    }

    public void CreateTile() 
    {
        Tile tile = Instantiate(_tilePrefab, _grid.transform);
        tile.SetStates(_tileStates[0], Consts.Numbers.NUMBER_2);
        //tile.SpawnTile();
    }
}
