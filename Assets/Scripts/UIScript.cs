using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
	public Detection detection;
	public Image progressBar;
	public Slice sliceObj;

	public RectTransform[] buttons;
	RectTransform hoveringButton = null;
	Camera cam;
	Vector3 pointerPos;
	float timer = 0f;
	bool onUI = false;

	private void Start()
	{
		cam = Camera.main;
	}
	private void PointerOnUI()
	{
		if (hoveringButton != null)
		{
			Rect rect = hoveringButton.rect;
			Vector3 pos = hoveringButton.position;
			if (pointerPos.x >= pos.x - rect.max.x && pointerPos.x <= pos.x + rect.max.x &&
				pointerPos.y >= pos.y - rect.max.y && pointerPos.y <= pos.y + rect.max.y)
			{
				onUI = true;
			}
			else
			{
				hoveringButton = null;
				onUI = false;
			}
		}
		else
		{
			foreach (RectTransform button in buttons)
			{
				Rect rect = button.rect;	
				Vector3 pos = button.position;
				if (pointerPos.x >= pos.x - rect.max.x && pointerPos.x <= pos.x + rect.max.x &&
					pointerPos.y >= pos.y - rect.max.y && pointerPos.y <= pos.y + rect.max.y)
				{
					hoveringButton = button;
					break;
				}
			}
		}
	}
	private void Update()
	{
		PointerOnUI();
		float duration = 4f;
		//Progress bar smooth movement
		float dist = Vector3.Distance(progressBar.transform.position, pointerPos);
		progressBar.transform.position = Vector3.MoveTowards(progressBar.transform.position, pointerPos, dist * sliceObj.speed * Time.deltaTime);

		pointerPos = detection.gPosition;
		if (onUI)
		{
			Image img = hoveringButton.GetComponent<Image>();
			img.fillAmount = 1 - (timer - 1f) / (duration - 1f);
			timer += Time.deltaTime;
			if(progressBar.gameObject.active)
			{
				progressBar.fillAmount = (timer - 1f) / (duration - 1f);
			}
		}
		else
		{
			timer = 0f;
			progressBar.fillAmount = 0f;
			progressBar.gameObject.SetActive(false);
		}
		if(timer > 1f)
		{
			progressBar.gameObject.SetActive(true);
		}
		if (timer > duration)
		{
			Invoke(hoveringButton.name, 0f);
		}
	}
	public void StartButton()
	{
		detection.webCamTexture.Stop();
		SceneManager.LoadScene("ChooseScene");
	}
	public void SliceIt()
	{
		detection.webCamTexture.Stop();
		SceneManager.LoadScene("SliceItScene");
	}
	public void Breakout()
	{
		detection.webCamTexture.Stop();
		SceneManager.LoadScene("BreakoutScene");
	}
	public void Hockey()
	{
		detection.webCamTexture.Stop();
		SceneManager.LoadScene("HockeyScene");
	}

	public void QuitButton()
	{
		print("Quit!");
		Application.Quit();
	}
}
