using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int score = 0;
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }
	private void Update()
	{
        scoreText.text = "Score: " + score.ToString();
	}
}
