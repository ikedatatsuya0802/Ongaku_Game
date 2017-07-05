using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {

	public GameObject ButtonA;
	public GameObject ButtonB;
	public GameObject ButtonC;
	public GameObject ButtonD;

	public GameObject[] LineEffect = new GameObject[4];

	bool[] pushButton = new bool[4];
	bool[] triggerButton = new bool[4];
	bool[] releaseButton = new bool[4];

	bool trigerCounter = false;

	// Use this for initialization
	void Start ()
	{
			
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(trigerCounter)
		{
			trigerCounter = false;
		}

		for(int i = 0; i < 4; i++)
		{
			if(triggerButton[i])
			{
				trigerCounter = true;
			}
		}

		if(!trigerCounter)
		{
			for(int i = 0; i < 4; i++)
			{
				triggerButton[i] = false;
				releaseButton[i] = false;
			}
		}
	}

	public bool GetButtonPush(int idx)
	{
		return pushButton[idx];
	}
	public bool GetButtonTrigger(int idx)
	{
		return triggerButton[idx];
	}
	public bool GetButtonRelease(int idx)
	{
		return releaseButton[idx];
	}

	public void PushButton(int button)
	{
		// ボタンフラグオン
		pushButton[button] = true;
		triggerButton[button] = true;

		// ラインエフェクト点灯
		LineEffect[button].GetComponent<MeshRenderer>().enabled = true;
	}

	public void ReleaseButton(int button)
	{
		// ボタンフラグオフ
		pushButton[button] = false;
		releaseButton[button] = true;

		// ラインエフェクト消灯
		LineEffect[button].GetComponent<MeshRenderer>().enabled = false;
	}
}
