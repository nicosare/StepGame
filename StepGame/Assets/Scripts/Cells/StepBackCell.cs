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
        if (PlayerInCell != null)
        {
            audioSource.PlayOneShot(actionSound,1);
            Message.Instance.LoadMessage("Вернись назад!");
            PlayerInCell.PenaltiesCount++;
            PlayerInCell.MakeStep(-stepBackCount);
            PlayerInCell = null;
        }
    }
}
