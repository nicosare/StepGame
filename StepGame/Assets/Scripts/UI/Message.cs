using System.Collections;
using TMPro;
using UnityEngine;

public class Message : MonoBehaviour
{
    [SerializeField]
    private Transform messageText;
    public static Message Instance;

    private void Awake()
    {
        Instance = this;
        messageText.gameObject.SetActive(false);
    }
    public void LoadMessage(string message, float seconds = 1)
    {
        StartCoroutine(ShowMessage(message, seconds));
    }

    IEnumerator ShowMessage(string message, float seconds)
    {
        messageText.gameObject.SetActive(true);
        messageText.GetComponent<TextMeshProUGUI>().text = message;
        yield return new WaitForSeconds(seconds);
        messageText.gameObject.SetActive(false);
    }
}
