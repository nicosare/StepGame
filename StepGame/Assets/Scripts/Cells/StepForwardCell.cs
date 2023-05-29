using UnityEngine;

public class StepForwardCell : Cell
{
    [SerializeField]
    private int stepForwardCount;
    private void Start()
    {
        Number.text = "+" + stepForwardCount;
    }

    protected override void CellAction()
    {
        if (PlayerInCell != null)
        {
            audioSource.PlayOneShot(actionSound,1);
            Message.Instance.LoadMessage("Отрывайся!");
            PlayerInCell.BonusesCount++;
            PlayerInCell.MakeStep(stepForwardCount);
            PlayerInCell = null;
        }
    }
}
