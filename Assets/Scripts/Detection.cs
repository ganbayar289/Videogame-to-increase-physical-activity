using UnityEngine;
using OpenCvSharp;

public class Detection : MonoBehaviour
{
    public WebCamTexture webCamTexture;
	public bool cameraSwitch = false;
	public bool isTwo = false;
	public Mat _frame;
	public Vector3 gPosition = new Vector3();
	public Vector3 oPosition = new Vector3();
	OpenCvSharp.Rect rect = new OpenCvSharp.Rect();
	OpenCvSharp.Rect rect1 = new OpenCvSharp.Rect();
	public Scalar gLower = new Scalar();
	public Scalar gUpper = new Scalar();
	public Scalar oLower = new Scalar();
	public Scalar oUpper = new Scalar();

	void Start()
    {
		WebCamDevice[] devices = WebCamTexture.devices;
		webCamTexture = new WebCamTexture(devices[0].name);
		webCamTexture.Play();
		gLower.Val0 = PlayerPrefs.GetInt("gx")-10;
		gLower.Val1 = PlayerPrefs.GetInt("gy") /2 - 10f;
		gLower.Val2 = PlayerPrefs.GetInt("gz") / 1.5 -10f;
		gUpper.Val0 = PlayerPrefs.GetInt("gx")+10;
		gUpper.Val1 = 255;
		gUpper.Val2 = 255;

		oLower.Val0 = PlayerPrefs.GetInt("ox") - 10;
		oLower.Val1 = PlayerPrefs.GetInt("oy") / 2 - 10f;
		oLower.Val2 = PlayerPrefs.GetInt("oz")  - 20f;
		oUpper.Val0 = PlayerPrefs.GetInt("ox") + 10;
		oUpper.Val1 = 255;
		oUpper.Val2 = 255;
		
	}

    void Update()
    {
		//Variables
		Mat output = new Mat();
		Mat mask = new Mat();
		Mat gMask = new Mat();
		Mat oMask = new Mat();
		Mat hierarchy = new Mat();
		Mat[] gContours = new Mat[10];
		Mat[] oContours = new Mat[10];
		Mat frame = OpenCvSharp.Unity.TextureToMat(webCamTexture);
		frame = Rescale(frame, 20);
		//OpenCV usage
		//Cv2.Blur(frame, frame, new Size(7, 7));
		Cv2.CvtColor(frame, output, ColorConversionCodes.BGR2HSV);
		Cv2.InRange(output, gLower, gUpper, gMask);
		gMask = Rescale(gMask, 500);
		Cv2.FindContours(gMask, out gContours, hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxNone);
		for (int i = 0; i < gContours.GetLength(0); i++)
		{
			if (gContours[i].ContourArea() > 5000f)
			{
				rect = Cv2.BoundingRect(gContours[i]);
				PositionCalculate(new Vector3(rect.X, rect.Y), new Vector3(rect1.X, rect1.Y));
			}
		}
		if (isTwo)
		{
			Cv2.InRange(output, oLower, oUpper, oMask);
			oMask = Rescale(oMask, 500);
			Cv2.FindContours(oMask, out oContours, hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxNone);
			for (int i = 0; i < oContours.GetLength(0); i++)
			{
				if (oContours[i].ContourArea() > 5000f)
				{
					rect1 = Cv2.BoundingRect(oContours[i]);
					PositionCalculate(new Vector3(rect.X, rect.Y), new Vector3(rect1.X, rect1.Y));
				}
			}
		}

		if (cameraSwitch && gameObject.GetComponent<MeshRenderer>().enabled)
		{
			//Cv2.BitwiseOr(oMask, gMask, mask);
			Show(gMask);
		}
		else if (gameObject.GetComponent<MeshRenderer>().enabled)
		{
			Show(OpenCvSharp.Unity.TextureToMat(webCamTexture));
		}
			
	}

	Mat Rescale(Mat frame, int percent)
	{
		int height = (int)(frame.Size().Height * percent / 100);
		int width = (int)(frame.Size().Width * percent / 100);
		Size size = new Size(width, height);
		Cv2.Resize(frame, frame, size, interpolation: InterpolationFlags.Area);
		return frame;
	}
	void Show(Mat frame)
	{
		//Detector as 3d object
		DestroyImmediate(GetComponent<Renderer>().material.mainTexture, true);
		//print(GetComponent<Renderer>().material.mainTexture);
		GetComponent<Renderer>().material.mainTexture = OpenCvSharp.Unity.MatToTexture(frame);

		//Detector as an ui element
		/*DestroyImmediate(texture, true);
		texture = OpenCvSharp.Unity.MatToTexture(frame);
		GetComponent<Image>().sprite = Sprite.Create(texture, new UnityEngine.Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));*/
	}

	void PositionCalculate(Vector3 gRect = new Vector3(), Vector3 oRect = new Vector3())
	{
		float width = Display.main.renderingWidth;
		float height = Display.main.renderingHeight;
		float ratio = width / height;
		gPosition = new Vector3(width - gRect.x * ratio, height - gRect.y * ratio);
		oPosition = new Vector3(width - oRect.x * ratio, height - oRect.y * ratio);
		//print("gPos: " + gPosition + ", oPos: " + oPosition);
	}
}
