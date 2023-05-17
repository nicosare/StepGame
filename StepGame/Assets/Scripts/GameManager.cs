using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine.SceneManagement;

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
    private bool playersReady;

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
            if (!currentPlayer.IsFinish)
                MakeTurn();
            else
                SwithToNextPlayer();
            if (Input.GetKeyDown(KeyCode.A))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);

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
        if (currentPlayer.Name == "Computer")
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
