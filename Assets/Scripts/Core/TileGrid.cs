using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public Cell[] cells { get; set; }
    public Row[] rows { get; set; }

    [SerializeField] private int size => cells.Length;
    [SerializeField] private int height => rows.Length;
    [SerializeField] private int width => size / height;

    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
        cells = GetComponentsInChildren<Cell>();
    }

    private void Start()
    {
        //Y AXIS 
        for (int i = 0; i < rows.Length; i++)
        {
            //X AXIS 
            for(int j = 0; j < rows[i].cells.Length; j++) 
            {
                rows[i].cells[j].coordinates = new Vector2Int(j, i);
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        if (x >= 0 && x < GetWidth() && y >= 0 && y < GetHeight())        
            return rows[y].cells[x];        
        else
            return null;
    }

    public Cell GetRandomEmptyCell()
    {
        int index = Random.Range(0, GetSize());
        int startingIndex = index;

        while (cells[index].isOccupied)
        {
            index++;
            if (index >= GetSize()) { index = 0; }
            if (index == startingIndex) { return null; }         
        }
        return cells[index];
    }

    public Cell GetAdjacentCell(Cell cell, Vector2Int direction) 
    {
        Vector2Int coordinates = cell.coordinates;

        coordinates.x += direction.x;
        coordinates.y -= direction.y;

        return GetCell(coordinates.x, coordinates.y);
    }

    public int GetSize() { return size; }
    public int GetHeight() { return height; }
    public int GetWidth() { return width;}

    
}
