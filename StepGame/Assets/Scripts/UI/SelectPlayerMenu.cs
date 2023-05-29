using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectPlayerMenu : MonoBehaviour
{
    [SerializeField]
    private PlayerPanel playerPanel;
    [SerializeField]
    private List<Player> players;
    private Transform addButton;
    private Transform playerPanelField;
    public List<PlayerPanel> panelsInGame;

    private void Start()
    {
        ResetPlayers();

        players.Shuffle();
        playerPanelField = transform.GetChild(1);
        addButton = transform.GetChild(2);
        for (int i = 0; i < 2; i++)
        {
            var firstPanel = Instantiate(playerPanel, transform.GetChild(1));
            firstPanel.Player = players[i];
            firstPanel.Background.color = players[i].MainColor;
            if (i == 1)
            firstPanel.ChangeButton.SetActive(true);
            panelsInGame.Add(firstPanel);
        }
        UpdatePanelsList();

    }

    private void Update()
    {
        if (playerPanelField.childCount < players.Count)
            addButton.gameObject.SetActive(true);
        else
        {
            addButton.gameObject.SetActive(false);
        }
    }

    public void UpdatePanelsList()
    {
        for (int i = 0; i < panelsInGame.Count; i++)
        {
            panelsInGame[i].RemovePlayerButton.SetActive(i == panelsInGame.Count - 1);
        }

        if (panelsInGame.Count > 0)
        {
            for (int i = 0; i < 2; i++)
                panelsInGame[i].RemovePlayerButton.SetActive(false);
        }
    }

    private void UpdatePlayers()
    {
        foreach (var playerPanel in panelsInGame)
        {
            if (playerPanel.Player.IsComputer)
                playerPanel.Player.PlayerName = "Компьютер " + panelsInGame.Where(panel => panel.Player.IsComputer && panel.Player.PlayerName == null).Count();
            else if (playerPanel.InputField.text != "")
                playerPanel.Player.PlayerName = playerPanel.InputField.text;
            else
                playerPanel.Player.PlayerName = "Игрок " + (panelsInGame.Where(panel => !panel.Player.IsComputer && panel.Player.PlayerName == null).Count());
            Debug.Log(playerPanel.Player.PlayerName);
        }
    }

    public void AddPlayer()
    {
        if (playerPanelField.childCount < players.Count)
        {
            var newPanel = Instantiate(playerPanel, playerPanelField);
            var player = players[newPanel.transform.GetSiblingIndex()];
            newPanel.Player = player;
            newPanel.Background.color = player.MainColor;
            newPanel.ChangeButton.SetActive(true);
            panelsInGame.Add(newPanel);
            UpdatePanelsList();
        }
    }

    public void LoadPlayers()
    {
        UpdatePlayers();
        panelsInGame.Reverse();
        foreach (var playerPanel in panelsInGame)
            PlayersManager.Instance.Players.Enqueue(playerPanel.Player);
    }

    private void ResetPlayers()
    {
        PlayersManager.Instance.FinishedPlayers.Clear();
        PlayersManager.Instance.Players.Clear();
        foreach (var player in players)
        {
            player.PlayerName = null;
            player.IsComputer = false;
            player.ColorInGame = player.MainColor;
        }
    }
}
