public class TeleportCell : Cell        // �� ��������� � ����. � ����������. 
{
    private TeleportCell otherTeleportCell;

    protected override void CellAction()
    {
        if (PlayerInCell != null)
        {
            FindOtherTeleportCell();

            PlayerInCell.CurrentPosition = int.Parse(otherTeleportCell.Number.text);
            PlayerInCell.transform.position = otherTeleportCell.transform.position;
            otherTeleportCell.PlayerInCell = PlayerInCell;
            PlayerInCell.MovesCount++;
            PlayerInCell.IsEndMove = true;
            PlayerInCell = null;
        }
    }

    private void FindOtherTeleportCell()
    {
        if (otherTeleportCell == null)
        {
            var teleportCells = FindObjectsOfType<TeleportCell>();

            foreach (var teleportCell in teleportCells)
            {
                if (teleportCell != this)
                    otherTeleportCell = teleportCell;
            }
        }
    }
}
