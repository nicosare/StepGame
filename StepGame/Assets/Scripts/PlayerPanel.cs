using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    public Player Player;
    public GameObject ComputerBlock;
    public GameObject PlayerBlock;
    public GameObject[] ChangeButtons;
    public Button[] RemovePlayerButtons;
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
}
