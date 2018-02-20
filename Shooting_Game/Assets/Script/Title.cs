using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

public class Title : MonoBehaviour
{
	ScreenManager sm;
	Fade fade;
	
	void Start()
	{
		sm		= GameObject.Find("ScreenManager").GetComponent<ScreenManager>();
		fade	= GameObject.Find("FadeCanvas").GetComponent<Fade>();

		// 画面タップでゲーム画面に遷移
		IObservable<Unit> tapDownStream = this.UpdateAsObservable().Where(_ => Input.GetMouseButtonDown(0));
		tapDownStream.Subscribe(_ => fade.FadeCall("game"));
	}
}