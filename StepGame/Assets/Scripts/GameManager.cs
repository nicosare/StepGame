using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void SetPlayerEvent();
    public delegate void MovePlayerEvent();
    public delegate void SecondTurnEvent();

    [SerializeField]
    private FieldBuilder fieldBuilder;
    [SerializeField]
    private Dice dice;
    [SerializeField]
    private Player currentPlayer;

    public Queue<Player> playersQueue;
    private bool playersReady;
    private List<Player> players;
    private void Awake()
    {
        dice.gameObject.SetActive(false); 
        players = DataManager.Instance.players;

    }

    private void Start()
    {
        fieldBuilder.GeneratePath();
        FieldBuilder.setPlayerEvent += SetPlayers;
        Dice.movePlayerEvent += PlayerTurn;
        Player.secondTurnEvent += SecondTurn;
    }

    private void SetPlayers()
    {
        
        playersQueue = new Queue<Player>();
        for (int i = 0; i < players.Count; i++)
        {
            var newPlayer = Instantiate(players[i]);
            newPlayer.transform.position = fieldBuilder.CellsPositions[0];
            newPlayer.Path = fieldBuilder.Path;
            playersQueue.Enqueue(newPlayer);
        }
        currentPlayer = playersQueue.Dequeue();
        playersReady = true;
    }

    private void Update()
    {
        if (CanMakeTurn())
        {
            if (!currentPlayer.IsFinish && !currentPlayer.IsSkipped)
            {
                MakeTurn();
            }
            else
            {
                currentPlayer.IsSkipped = false;
                SwithToNextPlayer();
            }
        }
    }

    private bool CanMakeTurn()
    {
        return !dice.isActiveAndEnabled
             && playersReady
             && playersQueue.All(player => player.IsEndMove);
    }

    private void MakeTurn()
    {
        if (currentPlayer.IsComputer)
            ThrowDice();
        else if (Input.GetKeyDown(KeyCode.Space))
            ThrowDice();
    }

    private void PlayerTurn()
    {
        if (dice.CurrentNumber == 0)
            ThrowDice();
        else
        {
            currentPlayer.MakeStep(dice.CurrentNumber);

            if (playersQueue.Count > 0)
            {
                SwithToNextPlayer();
            }
        }
    }

    private void SecondTurn()
    {
        for (int i = 0; i < playersQueue.Count; i++)
        {
            Debug.Log("Switch");
            SwithToNextPlayer();
        }
    }

    private void SwithToNextPlayer()
    {
        playersQueue.Enqueue(currentPlayer);
        currentPlayer = playersQueue.Dequeue();
    }

    private void ThrowDice()
    {
        dice.gameObject.SetActive(true);
        dice.Throw();
    }
}