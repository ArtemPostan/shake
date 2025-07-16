using System;
using System.IO;
using UnityEngine;

public class ScreenCapturer : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.P))
		{
			CaptureThisFrame();
		}
	}

	private void CaptureThisFrame()
	{
		Capture();
	}

	public static void Capture()
	{
		if (Directory.CreateDirectory("ScreenShots") == null)
		{
			Directory.CreateDirectory("ScreenShots");
		}
		ScreenCapture.CaptureScreenshot("ScreenShots/" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + ".png", 1);
	}
}
