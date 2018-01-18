using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletManager : MonoBehaviour
{
	[SerializeField]
	GameObject prefab;
		
	public static float speed	= 5.0f;

	void Awake()
	{
		
	}

	void Start()
	{
		
	}

	void Update()
	{
		
	}

	public void CreateBullet(float power, Vector2 pos, float angle, bool isEnemy = true)
	{
		GameObject instance = Instantiate(prefab, pos, Quaternion.Euler(Vector3.zero), transform);
		instance.name = "Bullet";
		instance.GetComponent<Bullet>().Shot(power, pos, angle, isEnemy);
	}
}