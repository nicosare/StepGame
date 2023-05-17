using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using static Cell;

public class Player : MonoBehaviour
{
    [SerializeField]
    private string name;
    private float speed = 5f;
    public int CurrentPosition;
    public bool IsEndMove;
    public bool IsFinish;
    public Dictionary<Vector3Int, Cell> Path;
    public string Name { get => name; set => name = value; }
    public static FitPlayersEvent fitPlayersEvent;

    private void Start()
    {
        IsEndMove = true;
        Path.First().Value.playersInCell.Add(this);
        FitPlayers();
    }

    private IEnumerator Move(List<Vector3Int> path)
    {
        IsEndMove = false;
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
                EndMove();
            }
        }
    }

    private void EndMove()
    {
        FitPlayers();
        IsEndMove = true;
    }

    public void MakeStep(int stepCount)
    {
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

    private static void FitPlayers()
    {
        if (fitPlayersEvent != null)
            fitPlayersEvent();
    }
}