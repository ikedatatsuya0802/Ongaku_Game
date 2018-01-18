using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
	RectTransform playerTrans;
	Player player;

	void Awake()
	{
		
	}

	void Start()
	{
		playerTrans = GameObject.Find("Player").GetComponent<RectTransform>();
		player = GameObject.Find("Player").GetComponent<Player>();
	}

	void Update()
	{
		
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		// エネミーの弾が当たった場合、弾を消去
		if(c.gameObject.tag == "EnemyBullet")
		{
			Bullet b = c.GetComponent<Bullet>();

			// 弾をプレイヤーが向いている角度の差
			float af = Mathf.Rad2Deg * (Mathf.Atan2(-(c.transform.localPosition.x - playerTrans.localPosition.x), (c.transform.localPosition.y - playerTrans.localPosition.y)));
			float pRot = player.angle * Mathf.Rad2Deg;
			float angle2bullet = af - pRot;
			
			if(angle2bullet > -230 && angle2bullet < -130)
			{
				Destroy(c.gameObject);
			}
		}
	}
}