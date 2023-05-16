using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class Dice : MonoBehaviour
{
    public static MovePlayerEvent movePlayerEvent;

    private Rigidbody rb;
    private bool isThrowing;
    public int CurrentNumber;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Throw()
    {
        transform.position = new Vector3(7.5f, 3, -2);
        transform.rotation = Quaternion.LookRotation(Random.insideUnitCircle);
        rb.AddForce(new Vector3(Random.Range(-5f, 5f), Random.Range(-1f, 2f), Random.Range(2f, 7f)));
        rb.AddTorque(Random.insideUnitSphere * Random.Range(5f, 10f), ForceMode.Impulse);
        isThrowing = true;
    }

    private void Update()
    {
        if (isThrowing && rb.IsSleeping())
        {
            isThrowing = false;
            CurrentNumber = GetNumber();

            if (movePlayerEvent != null)
            {
                movePlayerEvent();
            }

            gameObject.SetActive(false);
        }
    }

    public int GetNumber()
    {
        if (Vector3.Dot(transform.up, Vector3.up) > 0.9f)
        {
            Debug.Log("one");
            return 1;
        }
        else if (Vector3.Dot(-transform.right, Vector3.up) > 0.9f)
        {
            Debug.Log("two");
            return 2;
        }
        else if (Vector3.Dot(transform.forward, Vector3.up) > 0.9f)
        {
            Debug.Log("three");
            return 3;
        }
        else if (Vector3.Dot(-transform.forward, Vector3.up) > 0.9f)
        {
            Debug.Log("four");
            return 4;
        }
        else if (Vector3.Dot(transform.right, Vector3.up) > 0.9f)
        {
            Debug.Log("five");
            return 5;
        }
        else if (Vector3.Dot(-transform.up, Vector3.up) > 0.9f)
        {
            Debug.Log("six");
            return 6;
        }
        else
        {
            return 0;
        }
    }
}
