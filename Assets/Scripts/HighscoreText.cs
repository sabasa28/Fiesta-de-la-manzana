using TMPro;
using UnityEngine;

public class HighscoreText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highscoreText;
    void Start()
    {
        highscoreText.text = "Puntuación máxima: " + PlayerPrefs.GetInt("ConveyourHighscore", 0);
    }
}
