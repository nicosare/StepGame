using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Search;
using UnityEngine;
using static GameManager;

public class Player : MonoBehaviour
{
    [SerializeField]
    private string name;
    private float speed = 5f;
    public int CurrentPosition;
    public bool IsEndMove;

    public string Name { get => name; set => name = value; }

    private void Start()
    {
        IsEndMove = true;
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
                IsEndMove = true;
        }
    }

    public void MakeStep(int stepCount, List<Vector3Int> path)
    {
        var currentIndex = path.IndexOf(Vector3Int.RoundToInt(transform.position));
        var targetIndex = Mathf.Clamp(currentIndex + stepCount, 0, path.Count - 1);
        var newPath = new List<Vector3Int>();

        if (currentIndex < targetIndex)
            newPath = path.GetRange(currentIndex, targetIndex - currentIndex + 1);
        else
        {
            newPath = path.GetRange(targetIndex, currentIndex - targetIndex + 1);
            newPath.Reverse();
        }
        StartCoroutine(Move(newPath));

        CurrentPosition = Mathf.Clamp(CurrentPosition + stepCount, 0, path.Count - 1);
        Debug.Log(CurrentPosition);
    }
}