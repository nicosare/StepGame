using System.Collections;
using UnityEngine;

public class Decor : MonoBehaviour
{
    [SerializeField]
    private GameObject[] decorations;
    private void Start()
    {
        if (Random.Range(0, 101) < 25)
        {
            var newDecor = Instantiate(decorations[Random.Range(0, decorations.Length)], transform);
            newDecor.transform.localPosition += new Vector3(-5, -5, -5);
            StartCoroutine(SetDecor(newDecor));
        }
    }

    private IEnumerator SetDecor(GameObject decor)
    {
        var targetPosition = new Vector3(-5, 0, -5);
        while (decor.transform.localPosition != targetPosition)
        {
            decor.transform.localPosition = Vector3.MoveTowards(decor.transform.localPosition, targetPosition, Time.deltaTime * 10f);
            yield return null;
        }
    }
}
