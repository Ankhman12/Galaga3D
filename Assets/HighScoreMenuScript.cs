using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreMenuScript : MonoBehaviour
{
    [SerializeField] TMP_Text highScoreText;
    // Start is called before the first frame update
    void Awake()
    {
        highScoreText.text = "High Score:\n" + PlayerPrefs.GetInt("High Score", 0);
    }
}
