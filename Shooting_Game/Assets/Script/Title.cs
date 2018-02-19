using UnityEngine;

public class Title : MonoBehaviour
{
	ScreenManager sm;
	
	void Start()
	{
		sm = GameObject.Find("ScreenManager").GetComponent<ScreenManager>();
	}
	
	void Update()
	{
		if(Input.GetTouch(0).phase == TouchPhase.Began)
		{
			
		}
	}
}