using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    public Player Player;
    public GameObject ComputerBlock;
    public GameObject PlayerBlock;
    public GameObject ChangeButton;
    public GameObject RemovePlayerButton;
    public TMP_InputField InputField;
    public Image Background;

    private SelectPlayerMenu playerMenu;

    private void Start()
    {
        playerMenu = transform.parent.parent.GetComponent<SelectPlayerMenu>();
    }

    public void ChangePlayer()
    {
        if (PlayerBlock.activeSelf)
        {
            ComputerBlock.SetActive(true);
            PlayerBlock.SetActive(false);
            Player.IsComputer = true;
        }
        else
        {
            ComputerBlock.SetActive(false);
            PlayerBlock.SetActive(true);
            Player.IsComputer = false;
        }
    }

    public void RemovePlayer()
    {
        playerMenu.panelsInGame.Remove(this);
        playerMenu.UpdatePanelsList();
        Destroy(gameObject);
    }

    public void ChangeColor()
    {
        var newColor = new Color(Random.value, Random.value, Random.value);
        Background.color = newColor;
        Player.ColorInGame = newColor;
    }
}
