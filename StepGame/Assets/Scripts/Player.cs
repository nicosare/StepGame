using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Cell;
using static GameManager;

public class Player : MonoBehaviour
{
    public string PlayerName;
    private float speed = 5f;
    public int CurrentPosition;
    public bool IsEndMove;
    public bool IsFinish;
    public bool IsSkipped;
    public bool IsComputer;
    public Color MainColor;
    private MeshRenderer meshRenderer;
    public Dictionary<Vector3Int, Cell> Path;
    public static FitPlayersEvent fitPlayersEvent;
    public static CellActionEvent cellActionEvent;
    public static SecondTurnEvent secondTurnEvent;

    private void Start()
    {
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        meshRenderer.material.color = MainColor;
        IsEndMove = true;
        Path.First().Value.playersInCell.Add(this);
        FitPlayers();
    }

    private IEnumerator Move(List<Vector3Int> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            var targetPosition = path[i];

            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
                yield return null;
            }
            yield return new WaitForSeconds(.1f);
            if (i == path.Count - 1 && transform.position == targetPosition)
            {
                Path[targetPosition].playersInCell.Add(this);
                ApplyCellAction();
                FitPlayers();
            }
        }
    }

    public void MakeStep(int stepCount)
    {
        IsEndMove = false;

        var currentPosition = Vector3Int.RoundToInt(new Vector3(transform.position.x, 0, transform.position.z));
        transform.position = currentPosition;
        Path[currentPosition].playersInCell.Remove(this);
        FitPlayers();

        var currentIndex = Path.Keys.ToList().IndexOf(currentPosition);
        var targetIndex = Mathf.Clamp(currentIndex + stepCount, 0, Path.Count - 1);
        var newPath = GetPathDirection(currentIndex, targetIndex);
        StartCoroutine(Move(newPath));

        CurrentPosition = Mathf.Clamp(CurrentPosition + stepCount, 0, Path.Count - 1);
        if (CurrentPosition == Path.Count - 1)
            IsFinish = true;
    }

    private List<Vector3Int> GetPathDirection(int currentIndex, int targetIndex)
    {
        var pathKeys = Path.Keys.ToList();
        var minIndex = Math.Min(currentIndex, targetIndex);
        var maxIndex = Math.Max(currentIndex, targetIndex);
        var newPath = pathKeys.GetRange(minIndex, maxIndex - minIndex + 1);

        if (currentIndex > targetIndex)
            newPath.Reverse();
        return newPath;
    }

    public void TurnAgain()
    {
        if (secondTurnEvent != null)
            secondTurnEvent();
        IsEndMove = true;
    }

    private static void FitPlayers()
    {
        if (fitPlayersEvent != null)
            fitPlayersEvent();
    }

    private void ApplyCellAction()
    {
        if (cellActionEvent != null)
            cellActionEvent();
    }
}