using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Player : MonoBehaviour
{
	BulletManager	bm;
	GameObject		barrier;
	LifeGauge		lifegauge;
	Image           image;

	const float MAX_LIFE = 100;
	float life;
	public float angle;

	void Awake()
	{
		Input.gyro.enabled = true;
	}

	void Start()
	{
		bm			= GameObject.Find("BulletManager").GetComponent<BulletManager>();
		barrier		= transform.Find("Barrier").gameObject;
		image		= GetComponent<Image>();
		lifegauge	= GameObject.Find("LifeGauge").GetComponent<LifeGauge>();

		life = MAX_LIFE;
		angle = 0;
	}

	void Update()
	{		
		if(Input.gyro.attitude.x != 0)
		{
			Vector3 vecGyro = Input.gyro.attitude.eulerAngles;

			vecGyro.x -= vecGyro.x > 180 ? 360 : 0;
			vecGyro.y -= vecGyro.y > 180 ? 360 : 0;
			vecGyro.z = 0;
			vecGyro.x = Mathf.Ceil(vecGyro.x * 100) / 100.0f;
			vecGyro.y = Mathf.Ceil(vecGyro.y * 100) / 100.0f;
			vecGyro.Normalize();
			angle = Mathf.Atan2(-vecGyro.x, vecGyro.y);

			transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle * Mathf.Rad2Deg));
		}
		else
		{
			Vector3 mp = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
			Vector3 v = (mp - transform.localPosition).normalized;
			angle = Mathf.Atan2(-v.x, v.y);
			transform.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
		}
		

		if(Input.GetKeyDown(KeyCode.Space))
		{
			bm.CreateBullet(2.0f, 5.0f, transform.localPosition, -angle, false);
		}
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		// エネミーの弾が当たった場合、弾とプレイヤーを消去
		if(c.gameObject.tag == "EnemyBullet")
		{
			Bullet b = c.GetComponent<Bullet>();

			// ライフ減少処理
			life -= b.power;
			
			Destroy(c.gameObject);

			if(life <= 0)
			{
				Destroy(this.gameObject);

				// エフェクトでも出そう
			}
			else
			{
				image.DOFade(0, 0.2f).SetLoops(3).OnComplete(() => image.color = Color.white);
				lifegauge.Hit(life / MAX_LIFE);
			}
		}
	}

	public void CheckLive()
	{
		if(life > 0)
		{
			// シーン遷移
			GameObject.Find("FadeCanvas").GetComponent<Fade>().FadeCall("result", 0);
		}
	}
}