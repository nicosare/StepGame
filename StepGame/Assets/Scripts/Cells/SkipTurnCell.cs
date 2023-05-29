public class SkipTurnCell : Cell
{
    private void Start()
    {
        Number.text = "Ч";
    }

    protected override void CellAction()
    {
        if (PlayerInCell != null)
        {
            audioSource.PlayOneShot(actionSound, 1);
            Message.Instance.LoadMessage(PlayerInCell.PlayerName + " пропускает ход!");
            PlayerInCell.IsSkipped = true;
            PlayerInCell.MovesCount++;
            PlayerInCell.PenaltiesCount++;
            PlayerInCell.IsEndMove = true;
            PlayerInCell = null;
        }
    }
}
