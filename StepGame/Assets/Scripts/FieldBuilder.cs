using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class FieldBuilder : MonoBehaviour
{
    public static SetPlayerEvent setPlayerEvent;

    [SerializeField]
    private GameObject cell;
    [SerializeField]
    private int MinimalPathLength;
    [SerializeField]
    private int fieldWidth, fieldHeight;

    private bool[,] field;
    public List<Vector3Int> Path;
    public bool IsBuilded;

    private void Awake()
    {
        field = new bool[fieldWidth, fieldHeight];
        Path = new List<Vector3Int>();
    }

    public void GeneratePath()
    {
        var startPosition = GetRandomStartPosition();

        Path.Add(startPosition);
        field[startPosition.x, startPosition.z] = true;

        for (int i = 0; i < fieldWidth * fieldHeight - 1; i++)
        {
            Vector3Int nextPosition = GetRandomNextPosition(Path[i]);
            if (nextPosition == Vector3Int.zero)
                break;
            Path.Add(nextPosition);
            field[nextPosition.x, nextPosition.z] = true;
        }

        if (Path.Count < MinimalPathLength)
        {
            ClearField();
            GeneratePath();
        }
        else
            StartCoroutine(SpawnCells());
    }

    private void ClearField()
    {
        Path.Clear();
        field = new bool[fieldWidth, fieldHeight];
    }

    private Vector3Int GetRandomStartPosition()
    {
        return new Vector3Int(Random.Range(0, fieldWidth), 0, Random.Range(0, fieldHeight));
    }

    private Vector3Int GetRandomNextPosition(Vector3Int currentPosition)
    {
        var directions = new List<Vector3Int>() { Vector3Int.left, Vector3Int.right, Vector3Int.forward, Vector3Int.back };
        directions.Shuffle();

        foreach (var direction in directions)
        {
            var nextPosition = currentPosition + direction;

            if (InBounds(nextPosition) && !field[nextPosition.x, nextPosition.z])
                return nextPosition;
        }

        return Vector3Int.zero;
    }

    private bool InBounds(Vector3Int nextPosition)
    {
        return nextPosition.x >= 0
            && nextPosition.x < fieldWidth
            && nextPosition.z >= 0
            && nextPosition.z < fieldHeight;
    }


    private IEnumerator SpawnCells()
    {
        var i = 0;
        foreach (var position in Path)
        {
            var newCell = Instantiate(cell);
            newCell.transform.position = new Vector3Int(position.x, 0, position.z);
            newCell.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = i.ToString();
            i++;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.5f);
        SpawnDecor();
    }

    private void SpawnDecor()
    {
        if (setPlayerEvent != null)
        {
            setPlayerEvent();
        }
    }
}