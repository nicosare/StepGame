using System.Linq;

public class BasicCell : Cell
{
    protected override void CellAction()
    {
        if (playersInCell.Count > 0)
        {
            playersInCell.Last().IsEndMove = true;
        }
    }
}
