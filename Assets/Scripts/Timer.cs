using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
	public GameObject menu;
	public GameObject objectSpawner;
	public Score score;
    TextMeshProUGUI text;
	public float time = 60f;
	private void Start()
	{
		text = GetComponent<TextMeshProUGUI>();
	}
	void Update()
    {
		if(time <= 0)
		{
			menu.SetActive(true);
			objectSpawner.SetActive(false);
			text.text = "Time is up!";
			score.scoreText.text = "Your final score is " + score.score.ToString();
		}
		else
		{
			time -= Time.deltaTime;
			text.text = "Time left: " + ((int)time).ToString();
		}
    }
}
