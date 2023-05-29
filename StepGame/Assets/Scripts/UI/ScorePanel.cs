using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text place;
    [SerializeField]
    private TMP_Text playerName;
    [SerializeField]
    private TMP_Text movesCount;
    [SerializeField]
    private TMP_Text bonusesCount;
    [SerializeField]
    private TMP_Text penaltiesCount;
    [SerializeField]
    private Image background;

    public void ApplyPlayerData(string playerName, Color color, int movesCount, int bonusesCount, int penaltiesCount)
    {
        place.text = (transform.GetSiblingIndex() + 1).ToString();
        this.playerName.text = playerName;
        background.color = color;
        this.movesCount.text = movesCount.ToString();
        this.bonusesCount.text = bonusesCount.ToString();
        this.penaltiesCount.text = penaltiesCount.ToString();
    }
}
