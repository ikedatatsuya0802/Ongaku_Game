using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
	public float power = 0;

	public void Shot(float p, Vector3 pos, float angle, bool isEnemy = true)
	{
		// 弾のパワー設定
		power = p;

		// 座標初期化
		transform.localPosition = pos;

		// タグ付与
		gameObject.tag = isEnemy ? "EnemyBullet" : "PlayerBullet";
		
		// ショット方向設定
		Vector3 vec = new Vector3((Mathf.Sin(angle) * Screen.width * 2), Mathf.Cos(angle) * Screen.width * 2);
		transform.DOLocalMove(pos + vec, BulletManager.speed)
			.OnComplete(() =>
			{
				Destroy(this.gameObject);
			});

		GetComponent<Image>().color = isEnemy ? new Color(1.0f, 0.2f, 0.0f) : new Color(0.5f, 0.5f, 1.0f);
	}
}