using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesManager : MonoBehaviour {

	GameObject touchManager;
	TouchManager tm;
	
	Vector3 pos;
	Vector3 move;
	int lane;
	bool use = true;

	// Use this for initialization
	void Start ()
	{
		touchManager = GameObject.Find("TouchManager");
		tm = touchManager.GetComponent<TouchManager>();
		move = new Vector3(0, 0, -3);
		lane = 1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		pos = this.transform.position;

		if(tm.GetButtonTrigger(lane) && use)
		{
			if(BetweenE(pos.z, -10, 10))
			{
				move *= -2;
				use = false;
			}
		}


		pos += move;

		this.transform.position = pos;
	}


	//---------------------------------------------------------------------------------------
	// 範囲判定
	//---------------------------------------------------------------------------------------
	// 最少最大を含む範囲判定
	public bool BetweenE(float value, float val1, float val2)
	{
		return value <= Mathf.Max(val1, val2) && value >= Mathf.Min(val1, val2);
	}
	// 最少最大を含まない範囲判定
	public bool BetweenT(float value, float val1, float val2)
	{
		return value < Mathf.Max(val1, val2) && value > Mathf.Min(val1, val2);
	}
	// 最大を含み最少を含まない範囲判定
	public bool BetweenA(float value, float val1, float val2)
	{
		return value <= Mathf.Max(val1, val2) && value > Mathf.Min(val1, val2);
	}
}
