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

    private void Awake()
    {
        ResetPlayers();
    }

    private void Start()
    {
        players.Shuffle();
        playerPanelField = transform.GetChild(0);
        addButton = transform.GetChild(1);
        for (int i = 0; i < 2; i++)
        {
            var firstPanel = Instantiate(playerPanel, transform.GetChild(0));
            firstPanel.Player = players[i];
            firstPanel.Background.color = players[i].MainColor;
            if (i == 1)
                foreach (var button in firstPanel.ChangeButtons)
                    button.SetActive(true);
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
            foreach (var button in panelsInGame[i].RemovePlayerButtons)
                button.interactable = i == panelsInGame.Count - 1;
        }

        if (panelsInGame.Count > 0)
        {
            for (int i = 0; i < 2; i++)
                foreach (var button in panelsInGame[i].RemovePlayerButtons)
                    button.interactable = false;
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
                playerPanel.Player.PlayerName = "Игрок " + (panelsInGame.Where(panel => !panel.Player.IsComputer && panel.Player.PlayerName != null).Count() + 1);
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
            foreach (var button in newPanel.ChangeButtons)
                button.SetActive(true);
            panelsInGame.Add(newPanel);
            UpdatePanelsList();
        }
    }

    public void LoadPlayers()
    {
        UpdatePlayers();
        foreach (var playerPanel in panelsInGame)
            DataManager.Instance.players.Add(playerPanel.Player);
    }

    private void ResetPlayers()
    {
        foreach (var player in players)
        {
            player.PlayerName = null;
            player.IsComputer = false;
        }
    }
}
