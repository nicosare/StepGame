using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    private Rigidbody rb;
    public bool isThrowing;
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

            gameManager.PlayerTurn();

            gameObject.SetActive(false);
        }
    }

    public int GetNumber()
    {
        if (Vector3.Dot(transform.up, Vector3.up) > 0.9f)
        {
            return 1;
        }
        else if (Vector3.Dot(-transform.right, Vector3.up) > 0.9f)
        {
            return 2;
        }
        else if (Vector3.Dot(transform.forward, Vector3.up) > 0.9f)
        {
            return 3;
        }
        else if (Vector3.Dot(-transform.forward, Vector3.up) > 0.9f)
        {
            return 4;
        }
        else if (Vector3.Dot(transform.right, Vector3.up) > 0.9f)
        {
            return 5;
        }
        else if (Vector3.Dot(-transform.up, Vector3.up) > 0.9f)
        {
            return 6;
        }
        else
        {
            return 0;
        }
    }
}
