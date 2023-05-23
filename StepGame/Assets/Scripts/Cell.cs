using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Player;

public abstract class Cell : MonoBehaviour
{
    protected abstract void CellAction();
    public int CountCellsInGame;
    public List<Player> playersInCell;
    public TextMeshProUGUI Number;

    public delegate void FitPlayersEvent();
    public delegate void CellActionEvent();

    private void Awake()
    {
        fitPlayersEvent += FitPlayers;
        cellActionEvent += CellAction;
    }

    private void FitPlayers()
    {
        for (int i = 0; i < playersInCell.Count; i++)
        {
            playersInCell[i].transform.position = new Vector3(playersInCell[i].transform.position.x, 0.2f * i, playersInCell[i].transform.position.z);
        }
    }
}
