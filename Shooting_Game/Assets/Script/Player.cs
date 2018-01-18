using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Player : MonoBehaviour
{
	BulletManager	bm;
	GameObject		barrier;

	float life;
	public float angle;

	void Awake()
	{
		Input.gyro.enabled = true;
	}

	void Start()
	{
		bm = GameObject.Find("BulletManager").GetComponent<BulletManager>();
		barrier = transform.Find("Barrier").gameObject;
		life = 10;
		angle = 0;
	}

	void Update()
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

		if(Input.GetKey(KeyCode.Space))
		{
			bm.CreateBullet(2.0f, transform.localPosition, angle, false);
		}
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		// エネミーの弾が当たった場合、弾とプレイヤーを消去
		if(c.gameObject.tag == "EnemyBullet")
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