using UnityEngine;
using UnityEngine.UI;

public class SoundToggler : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sprites;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();

        if (AudioListener.volume == 0)
            image.sprite = sprites[0];
        else
            image.sprite = sprites[1];
    }

    public void OnOffMusic()
    {
        if (AudioListener.volume == 1)
        {
            image.sprite = sprites[0];
            AudioListener.volume = 0;
        }
        else
        {
            image.sprite = sprites[1];
            AudioListener.volume = 1;
        }
    }
}
