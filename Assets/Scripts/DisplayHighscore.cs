using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Changes the text of the object it is attached to to display the current highscore
/// i.e. Highscore: 100
/// </summary>
public class DisplayHighscore : MonoBehaviour
{

    /// <summary>
    /// The text component of our object
    /// </summary>
    private Text highscoreText;

    /// <summary>
    /// The Unity Start method
    /// </summary>
    void Start()
    {
        highscoreText = GetComponent<Text>();

        // As long as there is a highscore, display it
        if (PlayerPrefs.HasKey("Highscore"))
            highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore");
    }
}
