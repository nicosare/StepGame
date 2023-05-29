using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField]
    private ScorePanel scorePanel;
    [SerializeField]
    private Transform scoresList;
    public void DisplayScore()
    {
        var finishedPlayers = PlayersManager.Instance.FinishedPlayers;
        finishedPlayers.Sort((p1, p2) => p1.MovesCount.CompareTo(p2.MovesCount));

        foreach (var player in finishedPlayers)
        {
            var newScorePanel = Instantiate(scorePanel, scoresList);
            newScorePanel.ApplyPlayerData(player.PlayerName, player.ColorInGame, player.MovesCount, player.BonusesCount, player.PenaltiesCount);
        }
    }
}
