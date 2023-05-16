using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public delegate void SetPlayerEvent();
    public delegate void MovePlayerEvent();

    [SerializeField]
    private FieldBuilder fieldBuilder;
    [SerializeField]
    private Dice dice;
    [SerializeField]
    private List<Player> players;
    [SerializeField]
    private Player currentPlayer;

    private Queue<Player> playersQueue;
    private bool canThrowDice;

    private void Awake()
    {
        dice.gameObject.SetActive(false);
    }

    private void Start()
    {
        fieldBuilder.GeneratePath();
        FieldBuilder.setPlayerEvent += SetPlayers;
        Dice.movePlayerEvent += PlayerTurn;
    }

    private void SetPlayers()
    {
        players.Shuffle();
        playersQueue = new Queue<Player>();
        foreach (var player in players)
        {
            var newPlayer = Instantiate(player);
            newPlayer.transform.position = fieldBuilder.Path[0];
            playersQueue.Enqueue(newPlayer);
        }
        currentPlayer = playersQueue.Dequeue();
        canThrowDice = true;
    }

    private void Update()
    {
        if (!dice.isActiveAndEnabled
            && canThrowDice
            && playersQueue.All(player => player.IsEndMove))
        {
            if (currentPlayer.CurrentPosition != fieldBuilder.Path.Count - 1)
            {
                if (currentPlayer.Name == "Computer")
                    ThrowDice();
                else if (Input.GetKeyDown(KeyCode.Space))
                    ThrowDice();
            }
            else
                SkipTurn();
        }
    }

    private void PlayerTurn()
    {
        if (dice.CurrentNumber == 0)
            ThrowDice();
        else
        {
            currentPlayer.MakeStep(dice.CurrentNumber, fieldBuilder.Path);
            if (playersQueue.Count > 0)
            {
                playersQueue.Enqueue(currentPlayer);
                currentPlayer = playersQueue.Dequeue();
            }
        }
    }

    private void FitPlayers()
    {
        Debug.Log(playersQueue.Any(player => player.CurrentPosition == currentPlayer.CurrentPosition));
        if (playersQueue.Any(player => player.CurrentPosition == currentPlayer.CurrentPosition))
            currentPlayer.transform.GetChild(0).position += Vector3.up * 0.3f;
        else
            currentPlayer.transform.GetChild(0).position = new Vector3(currentPlayer.transform.position.x, 0.1f, currentPlayer.transform.position.z);
    }

    private void SkipTurn()
    {
        playersQueue.Enqueue(currentPlayer);
        currentPlayer = playersQueue.Dequeue();
    }

    private void ThrowDice()
    {
        Debug.Log("Throw");
        dice.gameObject.SetActive(true);
        dice.Throw();
    }
}
