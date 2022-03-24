using UnityEngine;
using OpenCvSharp;
using UnityEngine.UI;

public class Detection : MonoBehaviour
{
    public WebCamTexture webCamTexture;
	Texture2D texture;
	public bool cameraSwitch = false;
	public bool isTwo = false;
	public Scalar gLower = new Scalar(65, 70, 100);
	public Scalar gUpper = new Scalar(90, 255, 255);
	public Scalar oLower = new Scalar(0, 50, 240);
	public Scalar oUpper = new Scalar(30, 255, 255);
	public Vector3 gPosition = new Vector3();
	public Vector3 oPosition = new Vector3();
	OpenCvSharp.Rect rect = new OpenCvSharp.Rect();
	OpenCvSharp.Rect rect1 = new OpenCvSharp.Rect();


	void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
		webCamTexture = new WebCamTexture(devices[0].name);
		webCamTexture.Play();
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
		/*Cv2.CvtColor(frame, output, ColorConversionCodes.BGR2HSV);
		Cv2.InRange(output, gLower, gUpper, gMask);
		Cv2.InRange(output, oLower, oUpper, oMask);
		Cv2.BitwiseOr(gMask, oMask, mask);
		mask = Rescale(mask, 500);
		Cv2.FindContours(mask, out contours, hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxNone);
		for (int i = 0; i < contours.GetLength(0); i++)
		{
			if (contours[i].ContourArea() > 3000f)
			{
				OpenCvSharp.Rect rect = Cv2.BoundingRect(contours[i]);
				PositionCalculate(new Vector3(rect.X,rect.Y));
				//Cv2.Rectangle(frame, rect, new Scalar(255, 0, 0), thickness: 3);
			}
		}*/
		Cv2.CvtColor(frame, output, ColorConversionCodes.BGR2HSV);
		Cv2.InRange(output, gLower, gUpper, gMask);
		gMask = Rescale(gMask, 500);
		Cv2.FindContours(gMask, out gContours, hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxNone);
		for (int i = 0; i < gContours.GetLength(0); i++)
		{
			if (gContours[i].ContourArea() > 3000f)
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
				if (oContours[i].ContourArea() > 3000f)
				{
					rect1 = Cv2.BoundingRect(oContours[i]);
					PositionCalculate(new Vector3(rect.X, rect.Y), new Vector3(rect1.X, rect1.Y));
				}
			}
		}
		if (cameraSwitch)
			Show(mask);
		else
			Show(frame);
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
