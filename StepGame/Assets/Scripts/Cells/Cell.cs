using TMPro;
using UnityEngine;
using static Player;

public abstract class Cell : MonoBehaviour
{
    protected abstract void CellAction();
    protected AudioSource audioSource;
    public int CountCellsInGame;
    public Player PlayerInCell;
    public TextMeshProUGUI Number;
    public delegate void CellActionEvent();
    [SerializeField]
    protected AudioClip actionSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        cellActionEvent += CellAction;
    }


    protected void FitPlayers()
    {
        //for (int i = 0; i < PlayerInCell.Count; i++)
        //{
        //    PlayerInCell[i].transform.position = new Vector3(PlayerInCell[i].transform.position.x, 0.2f * i, PlayerInCell[i].transform.position.z);
        //}
    }
}
