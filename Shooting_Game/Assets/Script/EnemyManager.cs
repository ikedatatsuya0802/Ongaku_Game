using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyManager : MonoBehaviour
{
	[SerializeField]
	GameObject prefab;

	Sequence seq;
	

	void Start()
	{
		seq = DOTween.Sequence();

		seq.AppendCallback(() => CreateEnemy(0, new Vector2((Screen.width * 0.6f), 300), 10)).SetDelay(3.0f);

		seq.Play();
	}
	
	void Update()
	{
		
	}	

	// エネミー生成
	public void CreateEnemy(int type, Vector2 pos, float life)
	{
		GameObject instance = Instantiate(prefab, pos, Quaternion.Euler(Vector3.zero), transform);
		instance.name = "Enemy";
		instance.GetComponent<Enemy>().Init(0, pos, 10);
	}
}