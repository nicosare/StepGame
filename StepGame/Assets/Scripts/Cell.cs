using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Cell : MonoBehaviour
{
    protected abstract void CellAction();
    public List<Player> playersInCell;
    public TextMeshProUGUI Number;
    public Image Image;

    public delegate void FitPlayersEvent();

    private void Start()
    {
        Player.fitPlayersEvent += FitPlayers;
    }

    private void FitPlayers()
    {
        for (int i = 0; i < playersInCell.Count; i++)
        {
            playersInCell[i].transform.position = new Vector3(playersInCell[i].transform.position.x, 0.2f * i, playersInCell[i].transform.position.z);
        }
    }
}
