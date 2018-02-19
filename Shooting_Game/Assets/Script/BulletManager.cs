using UnityEngine;

public class BulletManager : SingletonMonoBehaviour<BulletManager>
{
	[SerializeField]
	GameObject PBulletPrefab;

	[SerializeField]
	GameObject EBulletPrefab;

	// プレハブから弾を生成・発射
	public void CreateBullet(float power, float speed, Vector2 pos, float angle, bool isEnemy = true)
	{
		GameObject instance = Instantiate((isEnemy ? EBulletPrefab : PBulletPrefab), pos, Quaternion.Euler(Vector3.zero), transform);
		instance.name = "Bullet";
		instance.GetComponent<Bullet>().Shot(power, speed, new Vector3(pos.x, pos.y, 1), angle, isEnemy);
	}
}