using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void SecondTurnEvent();

    [SerializeField]
    private FieldBuilder fieldBuilder;
    [SerializeField]
    private Dice dice;
    [SerializeField]
    private ScoreBoard scoreBoard;
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private TextMeshProUGUI playerTurnText;

    private Player currentPlayer;
    private bool playersReady;
    private Queue<Player> players;
    private bool isGameEnded;
    private int playersCount;
    private AudioSource audioSource;
    private List<Player> allPlayers;
    private IEnumerator checkTimeoutCoroutine;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        dice.gameObject.SetActive(false);
        players = PlayersManager.Instance.Players;
        playersCount = players.Count();
        checkTimeoutCoroutine = CheckTimeout();
    }

    private void Start()
    {
        fieldBuilder.GeneratePath();
        Player.secondTurnEvent += SecondTurn;

    }

    public void SetPlayers()
    {
        allPlayers = new List<Player>();
        allPlayers.Clear();
        for (int i = 0; i < playersCount; i++)
        {
            var newPlayer = Instantiate(players.Dequeue());
            allPlayers.Add(newPlayer);
            newPlayer.transform.position = fieldBuilder.CellsPositions[0];
            newPlayer.Path = fieldBuilder.Path;
            players.Enqueue(newPlayer);
        }
        currentPlayer = players.Dequeue();
        playersReady = true;
        FitPlayers();
        StartCoroutine(checkTimeoutCoroutine);
    }

    private void FitPlayers()
    {
        var groupedPlayers = allPlayers.GroupBy(player => player.CurrentPosition);
        foreach (var group in groupedPlayers)
        {
            var count = group.Count();
            for (int i = 0; i < count; i++)
            {
                group.ElementAt(i).transform.position = new Vector3(group.ElementAt(i).transform.position.x, 0.2f * i, group.ElementAt(i).transform.position.z);
            }
        }
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
            if (PlayersManager.Instance.FinishedPlayers.Count == playersCount && !isGameEnded)
            {
                playerTurnText.color = Color.white;
                playerTurnText.text = "Конец игры.";
                EndGame();
            }
            else if (!isGameEnded)
            {
                playerTurnText.color = currentPlayer.ColorInGame;
                playerTurnText.text = "Ходит " + currentPlayer.PlayerName + "...";
            }
        }
    }

    private bool CanMakeTurn()
    {
        return !dice.isActiveAndEnabled
             && playersReady
             && players.All(player => player.IsEndMove);
    }

    private void MakeTurn()
    {
        FitPlayers();
        if (currentPlayer.IsComputer)
            ThrowDice();
        else if (Input.GetKeyDown(KeyCode.Space))
            ThrowDice();
    }

    public void PlayerTurn()
    {
        if (dice.CurrentNumber == 0)
        {
            Message.Instance.LoadMessage("Неудачный бросок!");
            ThrowDice();
        }
        else
        {
            Message.Instance.LoadMessage(dice.CurrentNumber.ToString());
            currentPlayer.MakeStep(dice.CurrentNumber);

            if (players.Count > 0)
            {
                ResetTimeout();
                SwithToNextPlayer();
            }
        }
    }

    private void SecondTurn()
    {
        for (int i = 0; i < players.Count; i++)
            SwithToNextPlayer();
    }

    private void SwithToNextPlayer()
    {
        players.Enqueue(currentPlayer);
        currentPlayer = players.Dequeue();
        FitPlayers();
    }

    private void ThrowDice()
    {
        ResetTimeout();
        dice.gameObject.SetActive(true);
        dice.Throw();
    }

    private void EndGame()
    {
        audioSource.Play();
        isGameEnded = true;
        background.SetActive(true);
        scoreBoard.gameObject.SetActive(true);
        scoreBoard.DisplayScore();
    }
    private IEnumerator CheckTimeout()
    {
        yield return new WaitForSeconds(5f);
        if (!isGameEnded)
            Message.Instance.LoadMessage("[пробел] - бросить кубик", 2);
        checkTimeoutCoroutine = CheckTimeout();
        StartCoroutine(checkTimeoutCoroutine);
    }

    private void ResetTimeout()
    {
        StopCoroutine(checkTimeoutCoroutine);
        checkTimeoutCoroutine = CheckTimeout();
        StartCoroutine(checkTimeoutCoroutine);
    }
}