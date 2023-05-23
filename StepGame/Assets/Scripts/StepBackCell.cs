using System.Linq;
using UnityEngine;

public class StepBackCell : Cell
{
    [SerializeField]
    private int stepBackCount;
    private void Start()
    {
        Number.text = "-" + stepBackCount; 
    }

    protected override void CellAction()
    {
        if (playersInCell.Count > 0)
        {
            //playersInCell.Last().IsSkipped = true;
            //playersInCell.Remove(playersInCell.Last());
            playersInCell.Last().MakeStep(-stepBackCount);
            //playersInCell.Last().IsEndMove = true;
        }
    }
}
