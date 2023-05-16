using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private FieldBuilder fieldBuilder;
    [SerializeField]
    private Dice dice;
    [SerializeField]
    private List<PlayerMove> players;
    private PlayerMove currentPlayer;


    public delegate void SetPlayerEvent();
    public delegate void MovePlayerEvent();

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
        foreach (var player in players)
        {
            var newPlayer = Instantiate(player);
            newPlayer.transform.position = fieldBuilder.Path[0];
            currentPlayer = newPlayer;
        }
    }

    private void Update()
    {
        ThrowDice();
    }

    private void PlayerTurn()
    {
        currentPlayer.MakeStep(dice.CurrentNumber, fieldBuilder.Path);
    }

    private void ThrowDice()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dice.gameObject.SetActive(true);
            dice.Throw();
        }
    }
}
