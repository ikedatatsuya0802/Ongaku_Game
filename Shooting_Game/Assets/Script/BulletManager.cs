using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletManager : SingletonMonoBehaviour<BulletManager>
{
	[SerializeField]
	GameObject prefab;

	public void CreateBullet(float power, float speed, Vector2 pos, float angle, bool isEnemy = true)
	{
		GameObject instance = Instantiate(prefab, pos, Quaternion.Euler(Vector3.zero), transform);
		instance.name = "Bullet";
		instance.GetComponent<Bullet>().Shot(power, speed, pos, angle, isEnemy);
	}
}