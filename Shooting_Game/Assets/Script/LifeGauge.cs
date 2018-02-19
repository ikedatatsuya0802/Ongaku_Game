using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LifeGauge : MonoBehaviour
{
	RectTransform	prt;
	Image           image;
	Sequence        seq;

	void Start()
	{
		prt		= GameObject.Find("Player").GetComponent<RectTransform>();
		image	= GetComponent<Image>();
		
		// アルファを0に
		Color c = image.color;
		c.a = 0;
		image.color = c;		
	}

	void Update()
	{
		transform.localPosition = prt.localPosition;
	}

	// ライフ減少処理
	public void Hit(float life)
	{
		seq = DOTween.Sequence();
		seq.Append(image.DOColor(new Color(0, 0, 1, 1), 0.3f));
		seq.AppendInterval(1);
		seq.Append(image.DOColor(new Color(0, 0, 1, 0), 0.5f));
		seq.Play();
		image.DOFillAmount(life, 0.5f);
	}
}