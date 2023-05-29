public class SkipTurnCell : Cell
{
    private void Start()
    {
        Number.text = "�";
    }

    protected override void CellAction()
    {
        if (PlayerInCell != null)
        {
            audioSource.PlayOneShot(actionSound, 1);
            Message.Instance.LoadMessage(PlayerInCell.PlayerName + " ���������� ���!");
            PlayerInCell.IsSkipped = true;
            PlayerInCell.MovesCount++;
            PlayerInCell.PenaltiesCount++;
            PlayerInCell.IsEndMove = true;
            PlayerInCell = null;
        }
    }
}
