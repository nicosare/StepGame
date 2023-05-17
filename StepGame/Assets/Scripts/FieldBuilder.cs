using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class FieldBuilder : MonoBehaviour
{
    public static SetPlayerEvent setPlayerEvent;

    [SerializeField]
    private Cell cell;
    [SerializeField]
    private Decor decor;
    [SerializeField]
    private int MinimalPathLength;
    [SerializeField]
    private int MaximalPathLength;
    [SerializeField]
    private int fieldWidth, fieldHeight;

    private bool[,] field;
    public List<Vector3Int> CellsPositions;
    public Dictionary<Vector3Int, Cell> Path;
    public bool IsBuilded;

    private void Awake()
    {
        field = new bool[fieldWidth, fieldHeight];
        CellsPositions = new List<Vector3Int>();
        Path = new Dictionary<Vector3Int, Cell>();
    }

    public void GeneratePath()
    {
        var startPosition = GetRandomStartPosition();

        CellsPositions.Add(startPosition);
        field[startPosition.x, startPosition.z] = true;

        for (int i = 0; i < MaximalPathLength; i++)
        {
            Vector3Int nextPosition = GetRandomNextPosition(CellsPositions[i]);
            if (nextPosition == Vector3Int.zero)
                break;
            CellsPositions.Add(nextPosition);
            field[nextPosition.x, nextPosition.z] = true;
        }

        if (CellsPositions.Count < MinimalPathLength)
        {
            ClearField();
            GeneratePath();
        }
        else
            StartCoroutine(SpawnCells());
    }

    private void ClearField()
    {
        CellsPositions.Clear();
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

            if (InBounds(nextPosition) && !IsRoad(nextPosition) && !IsTouchRoad(currentPosition, nextPosition))
                return nextPosition;
        }

        return Vector3Int.zero;
    }

    private bool IsRoad(Vector3Int nextPosition)
    {
        return field[nextPosition.x, nextPosition.z];
    }

    private bool IsTouchRoad(Vector3Int currentPosition, Vector3Int nextPosition)
    {
        return (nextPosition.x > 0 && field[nextPosition.x - 1, nextPosition.z] && !(nextPosition.x - 1 == currentPosition.x && nextPosition.z == currentPosition.z)) ||
    (nextPosition.x < fieldWidth - 1 && field[nextPosition.x + 1, nextPosition.z] && !(nextPosition.x + 1 == currentPosition.x && nextPosition.z == currentPosition.z)) ||
    (nextPosition.z > 0 && field[nextPosition.x, nextPosition.z - 1] && !(nextPosition.x == currentPosition.x && nextPosition.z - 1 == currentPosition.z)) ||
    (nextPosition.z < fieldHeight - 1 && field[nextPosition.x, nextPosition.z + 1] && !(nextPosition.x == currentPosition.x && nextPosition.z + 1 == currentPosition.z));
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
        foreach (var position in CellsPositions)
        {
            var newCell = Instantiate(cell);
            newCell.transform.position = position;
            newCell.Number.text = CellsPositions.IndexOf(position).ToString();
            newCell.transform.SetParent(transform);
            Path.Add(position, newCell);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SpawnDecor());
    }
    private IEnumerator SpawnDecor()
    {
        for (int x = 0; x < fieldWidth; x++)
        {
            for (int z = fieldHeight - 1; z >= 0; z--)
            {
                if (!field[x, z])
                {
                    var newDecor = Instantiate(decor);
                    newDecor.transform.position = new Vector3(x, 0, z);
                    yield return new WaitForSeconds(0.005f);
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
        setPlayerEvent();
    }
}