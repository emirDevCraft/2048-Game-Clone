using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public TileStatesSO tileStatesSO { get; private set; }
    public Cell cell { get; private set; }
    public int number { get; private set; }

    [SerializeField] private Image _backgeoundImage;
    [SerializeField] private TextMeshProUGUI _numberText;

    public void SetStates(TileStatesSO tileStates, int number) 
    {
        this.tileStatesSO = tileStates;
        this.number = number;

        _backgeoundImage.color = tileStates.backgroundColor;
        _numberText.color = tileStates.textColor;
        _numberText.text = number.ToString();
    }

    public void SpawnTile(Cell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        this.cell = cell;
        this.cell.tile = this;

        transform.position = cell.transform.position;
    }
}
