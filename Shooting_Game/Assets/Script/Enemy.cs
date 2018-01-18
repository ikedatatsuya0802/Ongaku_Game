using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
	float life;
	BulletManager	bm;
	Transform pt;
	
	Sequence moveSeq;
	Sequence shotSeq;

	void Awake()
	{
		life = 0;	
	}

	void Start()
	{		
		bm = GameObject.Find("BulletManager").GetComponent<BulletManager>();
		pt = GameObject.Find("Player").transform;
	}

	// エネミーの初期化
	public void Init(int type, Vector3 pos, float l)
	{
		transform.localPosition = pos;

		// 動作セット
		SetMove(type);

		// ライフセット
		life = l;
	}

	void SetMove(int type)
	{
		moveSeq = DOTween.Sequence();
		shotSeq = DOTween.Sequence();

		switch(type)
		{
		case 0: // プレイヤーに弾を発射するだけ
			moveSeq.Append(transform.DOLocalMoveX(-500, 2.0f).SetEase(Ease.OutQuart).SetRelative());
			moveSeq.Join(transform.DOLocalMoveY(-300, 2.0f).SetRelative());
			moveSeq.Append(transform.DOLocalMoveX(500, 2.0f).SetEase(Ease.OutQuart).SetRelative());
			moveSeq.Join(transform.DOLocalMoveY(-300, 2.0f).SetRelative());
			
			shotSeq.AppendInterval(0.2f);
			shotSeq.AppendCallback(() => ShotToPlayer());
			shotSeq.SetLoops(15);
			break;
		case 1:
			break;
		case 2:
			break;
		case 3:
			break;
		}
		
		moveSeq.Play();
		shotSeq.Play();
	}

	void ShotToPlayer()
	{
		float angle = Mathf.Atan2((pt.localPosition.x - transform.localPosition.x), (pt.localPosition.y - transform.localPosition.y));
		bm.CreateBullet(2.0f, transform.localPosition, angle);
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		// プレイヤーの弾が当たった場合、弾とエネミーを消去
		if(c.gameObject.tag == "PlayerBullet")
		{
			Bullet b = c.GetComponent<Bullet>();

			life -= b.power;
			
			Destroy(c.gameObject);

			if(life <= 0)
			{
				Destroy(this.gameObject);

				// エフェクトでも出そう
			}
		}
	}
}