using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using OpenCvSharp;

public class UIScript : MonoBehaviour
{
	public Detection detection;
	public Image progressBar;
	public Slice sliceObj;
	public GameObject capture;
	public GameObject capture1;
	public RectTransform place;
	public GameObject instruction;
	public GameObject instruction1;
	public Material plane;

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
			UnityEngine.Rect rect = hoveringButton.rect;
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
				UnityEngine.Rect rect = button.rect;	
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
		if(gameObject.activeSelf)
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
				if (progressBar.gameObject.activeSelf)
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
			if (timer > 1f)
			{
				progressBar.gameObject.SetActive(true);
			}
			if (timer > duration)
			{
				Invoke(hoveringButton.name, 0f);
			}
		}
	}
	public void StartButton()
	{
		detection.webCamTexture.Stop();
		SceneManager.LoadScene("ChooseScene");
	}
	public void Menu()
	{
		detection.webCamTexture.Stop();
		SceneManager.LoadScene("MenuScene");
	}
	public void SliceIt()
	{
		detection.webCamTexture.Stop();
		SceneManager.LoadScene("SliceItScene");
	}
	public void Calibrate()
	{
		detection.webCamTexture.Stop();
		SceneManager.LoadScene("CalibrateScene");
	}
	public void _Calibrate()
	{
		Invoke("Calibrate", 0f);
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
	public void CaptureFirst()
	{
		capture.SetActive(false);
		capture1.SetActive(true);
		instruction.SetActive(false);
		instruction1.SetActive(true);

		Vector4 meanColor = MeanColorInHSV();
		PlayerPrefs.SetInt("gx", (int)meanColor.x);
		PlayerPrefs.SetInt("gy", (int)meanColor.y);
		PlayerPrefs.SetInt("gz", (int)meanColor.z);
	}
	public void CaptureSecond()
	{
		Vector4 meanColor = MeanColorInHSV();
		PlayerPrefs.SetInt("ox", (int)meanColor.x);
		PlayerPrefs.SetInt("oy", (int)meanColor.y);
		PlayerPrefs.SetInt("oz", (int)meanColor.z);
		Invoke("Menu",0f);
	}
	Vector4 MeanColorInHSV()
	{
		Mat frame = OpenCvSharp.Unity.TextureToMat(detection.webCamTexture);
		frame = frame[new Range(frame.Height - (int)((place.transform.position.y + place.rect.yMax) / 1.5f),
			frame.Height - (int)((place.transform.position.y - place.rect.yMax) / 1.5f)),
			new Range(frame.Width - (int)((place.transform.position.x + place.rect.xMax) / 1.5f),
			frame.Width - (int)((place.transform.position.x - place.rect.xMax) / 1.5f))];
		DestroyImmediate(plane.mainTexture, true);
		plane.mainTexture = OpenCvSharp.Unity.MatToTexture(frame);
		Scalar mean = frame.Mean();
		Vector4 meanColor = new Vector4((float)mean.Val2, (float)mean.Val1, (float)mean.Val0, (float)mean.Val3);
		Color.RGBToHSV((Color)meanColor, out meanColor.x, out meanColor.y, out meanColor.z);
		meanColor.x *= 180;
		meanColor.y *= 255;
		return new Vector4(meanColor.x, meanColor.y, meanColor.z);
	}

	public void QuitButton()
	{
		print("Quit!");
		Application.Quit();
	}
}
