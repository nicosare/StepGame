using System.Linq;

public class SecondTurnCell : Cell
{
    private void Start()
    {
        Number.text = "+";
    }

    protected override void CellAction()
    {
        if (playersInCell.Count > 0)
        {
            playersInCell.Last().TurnAgain();
        }
    }
}
