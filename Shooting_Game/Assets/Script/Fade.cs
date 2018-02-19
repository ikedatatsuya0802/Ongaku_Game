using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fade : SingletonMonoBehaviour<Fade>
{
	ScreenManager	sm;

	[SerializeField]
	bool loadtextBeing = true;

	Text t;
    
	float FadeTime      = 2.0f; // フェードに掛かる時間
    float TimeAfterFade = 3.0f; // シャッター後ロードまでの時間

    Image fadeImage;

	int fadeType = 0;

	bool isFading;  // フェード中か否か
    
	Sequence textSeq;
	
	void Awake()
	{
		if(CheckInstance())
		{
			DontDestroyOnLoad(transform.root.gameObject);
		}
    }
		
	void Start()
	{
		// ゲームオブジェクト取得
		sm						= GameObject.Find("ScreenManager").GetComponent<ScreenManager>();
		fadeImage				= transform.Find("Fade").GetComponent<Image>();

		t			= transform.Find("LoadText").GetComponent<Text>();
		isFading	= false;
		t.color		= new Color(1, 1, 1, 0);
		
		// テキスト表示用のシーケンス設定
		textSeq = DOTween.Sequence();
		textSeq.PrependCallback(() => t.text = "Now Loading");
		textSeq.Append(t.DOText("Now Loading", 0)).AppendInterval(0.2f);
		textSeq.Append(t.DOText("Now Loading.", 0)).AppendInterval(0.2f);
		textSeq.Append(t.DOText("Now Loading..", 0)).AppendInterval(0.2f);
		textSeq.Append(t.DOText("Now Loading...", 0)).AppendInterval(0.2f);
		textSeq.OnPause(() => t.color = new Color(1, 1, 1, 0));
		textSeq.SetLoops(-1);
	}

	
	
	public void FadeCall(string fadeSceneName, int type, float fadeTime = 0, float timeAfterFade = 0)
	{
		if(fadeTime > 0.1f) FadeTime = fadeTime;
		if(timeAfterFade > 0.1f) TimeAfterFade = timeAfterFade;

		if(!isFading)
		{
			Sequence seq		= DOTween.Sequence();
			Sequence seq2		= DOTween.Sequence();
			fadeType = type;

			switch(type)
			{
				case 0:
					seq.PrependCallback(() => fadeImage.color = new Color(1, 1, 1, 0));
					seq.PrependCallback(() => isFading = true);
					seq.Append(fadeImage.DOColor(Color.white, FadeTime));
					break;
				default:
					break;
			}

			if(loadtextBeing) seq.Append(t.DOColor(Color.white, 1.0f).OnComplete(() => textSeq.Play()));

            seq.AppendInterval(TimeAfterFade);
            seq.OnComplete(() => sm.LoadSceneAsync(fadeSceneName, FadeFinish));
			

			seq.Play();
		}
	}

	void FadeFinish()
	{
		Sequence seq = DOTween.Sequence();
		
		if(loadtextBeing) textSeq.Pause();
				
		switch(fadeType)
		{
			case 0:
				seq.PrependCallback(() => fadeImage.color = Color.white);
				seq.Append(fadeImage.DOColor(new Color(1, 1, 1, 0), FadeTime));
				break;
			default:
				break;
		}
		seq.OnComplete(() => isFading = false);
				
		seq.Play();
	}
}
