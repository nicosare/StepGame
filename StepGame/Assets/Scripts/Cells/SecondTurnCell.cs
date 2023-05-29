public class SecondTurnCell : Cell
{
    private void Start()
    {
        Number.text = "+";
    }

    protected override void CellAction()
    {
        if (PlayerInCell != null)
        {
            audioSource.PlayOneShot(actionSound, 1);
            Message.Instance.LoadMessage("Дополнительный ход!");
            PlayerInCell.MovesCount++;
            PlayerInCell.BonusesCount++;
            PlayerInCell.TurnAgain();
            PlayerInCell = null;
        }
    }
}
