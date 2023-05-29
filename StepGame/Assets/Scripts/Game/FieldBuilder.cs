using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldBuilder : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private List<Cell> cells;
    [SerializeField]
    private Decor decor;
    [SerializeField]
    private int MinimalPathLength;
    [SerializeField]
    private int MaximalPathLength;
    [SerializeField]
    private int fieldWidth, fieldHeight;

    private bool[,] field;
    private List<Cell> preparedCells;
    public List<Vector3Int> CellsPositions;
    public Dictionary<Vector3Int, Cell> Path;
    public bool IsBuilded;

    private void Awake()
    {
        field = new bool[fieldWidth, fieldHeight];
        CellsPositions = new List<Vector3Int>();
        preparedCells = new List<Cell>();
        Path = new Dictionary<Vector3Int, Cell>();
        cells.Sort((c1, c2) => c1.CountCellsInGame.CompareTo(c2.CountCellsInGame));
    }

    private void PrepareCells()
    {
        for (int i = 1; i < cells.Count && preparedCells.Count < CellsPositions.Count; i++)
        {
            for (int j = 0; j < cells[i].CountCellsInGame; j++)
            {
                preparedCells.Add(cells[i]);
            }
        }

        while (preparedCells.Count < CellsPositions.Count)
        {
            preparedCells.Add(cells.First());
        }

        preparedCells.Shuffle();
        StartCoroutine(SpawnCells());
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
            PrepareCells();
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
        for (int i = 0; i < CellsPositions.Count; i++)
        {
            var newCell = i != 0 && i != CellsPositions.Count - 1 ? Instantiate(preparedCells[i]) : Instantiate(cells.First());
            newCell.transform.position = CellsPositions[i];
            newCell.transform.SetParent(transform);
            Path.Add(CellsPositions[i], newCell);
            newCell.Number.text = CellsPositions.IndexOf(CellsPositions[i]).ToString();
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
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
        gameManager.SetPlayers();
    }
}