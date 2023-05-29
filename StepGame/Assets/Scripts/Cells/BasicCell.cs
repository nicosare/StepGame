public class BasicCell : Cell
{
    protected override void CellAction()
    {
        if (PlayerInCell != null)
        {
            if (PlayerInCell.IsFinish)
            {
                audioSource.PlayOneShot(actionSound, 1);
                Message.Instance.LoadMessage(PlayerInCell.PlayerName + " финишировал!");
            }
            PlayerInCell.MovesCount++;
            PlayerInCell.IsEndMove = true;
            PlayerInCell = null;
        }
    }
}
