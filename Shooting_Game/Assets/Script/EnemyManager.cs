using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
	[SerializeField]
	GameObject prefab;

	[SerializeField]
	int stage = 1;

	Sequence arrangeSeq;
	EnemyMotionData[] motionArray;
	
	void Awake()
	{
		// シーケンス生成
		arrangeSeq = DOTween.Sequence();
		
		TextAsset[] tas = Resources.LoadAll<TextAsset>("EnemyScript");
	}

	void Start()
	{
		// エネミーモーションモーション読み込み
		ReadEnemyMotionData();

		// ステージデータ読み込み
		ReadArrangement();
	}

	// エネミー生成
	public void CreateEnemy(Vector2 pos, float life, EnemyMotionData emd)
	{
		GameObject instance = Instantiate(prefab, pos, Quaternion.Euler(Vector3.zero), transform);
		instance.name = "Enemy";
		instance.GetComponent<Enemy>().Init(pos, 10, emd);
	}

	// エネミーの配置情報を読み取り
	void ReadArrangement()
	{
		// スクリプト読み込み
		TextAsset StageScript = Resources.Load<TextAsset>("StageScript/Stage" + stage);
		string[] strData = StageScript.text.Split(new string[]{Environment.NewLine}, 0);
		
		// 行(データ)単位でデータ読み出し
		for(int i = 0 ; i < strData.Length ; i++)
		{	
			// 読み出しデータより処理実行
			switch(strData[i].Split(':')[0])
			{
			case "ENEMY":	// エネミー生成

				EnemyData ed = ReadArrangementLine(strData[i]);
				arrangeSeq.AppendInterval(ed.time - arrangeSeq.Duration());
				arrangeSeq.AppendCallback(() =>
				CreateEnemy(new Vector2(ed.pos.x, ed.pos.y), ed.life, motionArray[ed.type]));
				break;
			case "END":     // ステージ終了

				// シーン遷移処理
				float endTime = float.Parse(strData[i].Split(':')[1]);
				arrangeSeq.AppendInterval(endTime);
				arrangeSeq.AppendCallback(()
					=> GameObject.Find("Player").GetComponent<Player>().CheckLive());
				break;
			default:
				break;
			}
		}

		arrangeSeq.Play();
	}

	// 行単位の配置情報を処理
	EnemyData ReadArrangementLine(string str)
	{
		EnemyData ed = new EnemyData();
		string[] s = new string[5];

		// 行単位のデータを読み込み
		s = str.Split(':')[1].Split(new string[]{", "}, 0);
		ed.time = float.Parse(s[0]);
		ed.type = int.Parse(s[1]);
		ed.pos = new Vector2(float.Parse(s[2]), float.Parse(s[3]));
		ed.life = int.Parse(s[4]);
		
		return ed;
	}

	// モーション読み取り
	void ReadEnemyMotionData()
	{
		TextAsset[] tas = Resources.LoadAll<TextAsset>("EnemyScript");
		motionArray = new EnemyMotionData[tas.Length];

		for(int i = 0 ; i < motionArray.Length ; i++)
		{
			motionArray[i] = new EnemyMotionData();

			// エネミー個別のデータ読み出し
			string[] line = tas[i].text.Split(new string[]{Environment.NewLine}, 0);
			
			for(int j = 0 ; j < line.Length ; j++)
			{
				switch(line[j].Split(':')[0])
				{
				case "MOVE":	// 移動データを追加
					motionArray[i].move.Add(ReadEnemyMove(line[j].Split(':')[1]));
					break;
				case "SHOT1":	// 対プレイヤー弾データを追加
					motionArray[i].shotP.Add(ReadEnemyShot(line[j].Split(':')[1]));
					break;
				case "SHOT2":	// 自由弾データを追加
					motionArray[i].shotF.Add(ReadEnemyShot(line[j].Split(':')[1]));
					break;
				case "ESCAPE":	// 逃げる時間
					motionArray[i].escTime = float.Parse(line[j].Split(':')[1]);
					break;
				}
			}
		}
	}

	// 移動シーケンスを読み取り
	EnemyMove ReadEnemyMove(string line)
	{
		string[] s = line.Split(new string[]{", "}, 0);
		EnemyMove em = new EnemyMove();

		
		em.execTime	= float.Parse(s[0]);	// 移動を開始する時間
		em.flowTime	= float.Parse(s[1]);	// 移動に掛ける時間
		em.move		= new Vector2(float.Parse(s[2]), float.Parse(s[3]));	// 移動量

		return em;
	}

	// ショットデータを読み取り
	EnemyShot ReadEnemyShot(string line)
	{
		string[] s = line.Split(new string[]{", "}, 0);
		EnemyShot es = new EnemyShot();
		
		es.execTime		= float.Parse(s[0]);
		es.power		= float.Parse(s[1]);
		es.speed		= float.Parse(s[2]);
		es.angleDeg		= float.Parse(s[3]);

		return es;
	}
}

// 生成のためのデータ
public class EnemyData
{
	public float time;
	public int type;
	public Vector2 pos;
	public int life;

	public EnemyData()
	{
		time	= 0;
		type	= 0;
		pos	= Vector2.zero;
		life	= 0;
	}
}

// モーションクラス
public class EnemyMotionData
{
	// シーケンスでないのは、読み取り段階ではエネミー・プレイヤーのトランスフォームが存在しないため
	public List<EnemyMove>	move	= new List<EnemyMove>();	// 移動データ
	public List<EnemyShot>	shotP	= new List<EnemyShot>();	// 対プレイヤー弾のショットデータ
	public List<EnemyShot>	shotF	= new List<EnemyShot>();    // 自由発射弾のショットデータ
	public float            escTime = 0;
}

// 移動データクラス
public class EnemyMove
{
	public float execTime	= 0;
	public float flowTime	= 0;
	public Vector2 move		= Vector2.zero;
}

// ショットデータクラス
public class EnemyShot
{
	public float execTime	= 0;
	public float power		= 0;
	public float speed		= 0;
	public float angleDeg	= 0;
}