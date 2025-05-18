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

    public int GetSize() { return size; }
    public int GetHeight() { return height; }
    public int GetWidth() { return width;}
}
