using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    TextMeshProUGUI text;
	float time = 60f;
	private void Start()
	{
		text = GetComponent<TextMeshProUGUI>();
	}
	void Update()
    {
		time -= Time.deltaTime;
		text.text = "Time left: " + ((int)time).ToString();
		if(time < 1)
		{

		}
    }
}
