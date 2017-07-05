using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodTouches;

public class TouchScript : MonoBehaviour {

	public GameObject go;
	Vector3 pos;

	// Use this for initialization
	void Start ()
	{
		pos = go.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(true)
		{
			pos.z += 1.0f;
			go.transform.position = pos;
		}
	}
}
