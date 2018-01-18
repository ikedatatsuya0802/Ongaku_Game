using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debugger : MonoBehaviour
{
	Text mousePosText;
	Text FPSText;
	Text DebugText;

	int FPS = 30;
	int frameCount	= 0;
	float prevTime	= 0;
	
	void Start()
	{
		mousePosText	= transform.Find("MousePos").GetComponent<Text>();
		FPSText			= transform.Find("FPS").GetComponent<Text>();
		DebugText		= transform.Find("DebugText").GetComponent<Text>();

		frameCount	= 0;
		prevTime	= 0;
	}


	void Update()
	{
		++frameCount;
		float time = Time.realtimeSinceStartup - prevTime;

		mousePosText.text = "mousePosX:" + (Input.mousePosition.x - Screen.width / 2) + "\n"
			+ "mousePosY:" + (Input.mousePosition.y - Screen.height / 2);

		FPSText.text = "FPS:" + FPS;

		if(time >= 0.5f)
		{
			FPS = (int)(frameCount / time);
			frameCount = 0;
			prevTime = Time.realtimeSinceStartup;
		}

		DebugText.text = "";
	}

	public void DebuggerTextAdd(string t)
	{
		DebugText.text += t;
	}
}