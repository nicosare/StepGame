using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject cell;
    [SerializeField]
    private int MinimalPathLength;
    [SerializeField]
    private int fieldWidth, fieldHeight;

    private bool[,] field; // Используем булевый массив для отслеживания посещенных клеток
    private List<Vector3Int> path; // Список всех позиций дороги

    private void Awake()
    {
        field = new bool[fieldWidth, fieldHeight];
        path = new List<Vector3Int>();
    }

    private void Start()
    {
        GeneratePath();
    }

    private void GeneratePath()
    {
        var startPosition = GetRandomStartPosition();

        path.Add(startPosition);
        field[startPosition.x, startPosition.z] = true;

        for (int i = 0; i < fieldWidth * fieldHeight - 1; i++)
        {
            Vector3Int nextPosition = GetRandomNextPosition(path[i]);
            if (nextPosition == Vector3Int.zero) // Если не удалось найти позицию, заканчиваем генерацию
                break;
            path.Add(nextPosition);
            field[nextPosition.x, nextPosition.z] = true;
        }

        if (path.Count < MinimalPathLength)
        {
            ClearField();
            GeneratePath();
        }
        else
            StartCoroutine(SpawnCells());
    }

    private void ClearField()
    {
        path.Clear();
        field = new bool[fieldWidth, fieldHeight];
    }

    private Vector3Int GetRandomStartPosition()
    {
        return new Vector3Int(Random.Range(0, fieldWidth), 0, Random.Range(0, fieldHeight));
    }

    private Vector3Int GetRandomNextPosition(Vector3Int currentPosition)
    {
        var directions = new List<Vector3Int>() { Vector3Int.left, Vector3Int.right, Vector3Int.forward, Vector3Int.back };
        directions.Shuffle(); // Перемешиваем список направлений

        foreach (var direction in directions)
        {
            var nextPosition = currentPosition + direction;

            if (InBounds(nextPosition)
                && !field[nextPosition.x, nextPosition.z])
            //&& !IntersectsWithSelf(nextPosition))
            {
                return nextPosition;
            }
        }

        return Vector3Int.zero; // Не удалось найти подходящую позицию
    }

    private bool InBounds(Vector3Int nextPosition)
    {
        return nextPosition.x >= 0
            && nextPosition.x < fieldWidth
            && nextPosition.z >= 0
            && nextPosition.z < fieldHeight;
    }

    private bool IntersectsWithSelf(Vector3Int nextPosition)
    {
        // Проверяем, совпадает ли найденная позиция с любой из предыдущих позиций
        for (int i = 0; i < path.Count - 1; i++)
        {
            if (nextPosition == path[i])
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator SpawnCells()
    {
        var i = 0;
        foreach (var position in path)
        {
            var newCell = Instantiate(cell);
            newCell.transform.position = new Vector3Int(position.x, 0, position.z);
            newCell.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = i.ToString();
            i++;
            yield return new WaitForSeconds(0.05f);
        }
    }
}