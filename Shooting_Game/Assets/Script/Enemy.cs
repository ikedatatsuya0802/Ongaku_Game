using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
	[SerializeField]
	AnimationCurve curve;

	float			life = 0;
	float			maxLife = 0;
	BulletManager	bm;
	Transform		pt;
	Image			lifeImage;
	
	Motion motion;		// モーション

	void Awake()
	{		
		bm = GameObject.Find("BulletManager").GetComponent<BulletManager>();
		pt = GameObject.Find("Player") ? GameObject.Find("Player").transform : null;
		lifeImage = transform.Find("Life").GetComponent<Image>();

		// シーケンス初期化
		motion = new Motion();
		motion.move		= DOTween.Sequence();
		motion.shotP	= DOTween.Sequence();
		motion.shotF	= DOTween.Sequence();
	}
	
	void Start()
	{

	}

	// エネミーの初期化
	public void Init(Vector3 pos, float l, EnemyMotionData emd)
	{
		transform.localPosition = pos;

		// 動作セット
		SetMove(emd);

		// ライフセット
		life = l;
		maxLife = l;
	}

	void SetMove(EnemyMotionData emd)
	{
		foreach(EnemyMove em in emd.move)
		{
			motion.move.AppendInterval(em.execTime - motion.move.Duration());
			motion.move.Append(transform.DOLocalMove(em.move, em.flowTime)).SetRelative();
		}

		foreach(EnemyShot es in emd.shotP)
		{
			motion.shotP.AppendInterval(es.execTime - motion.shotP.Duration());
			motion.shotP.AppendCallback(() => ShotToPlayer((es.angleDeg * Mathf.Deg2Rad), es.power, es.speed));
		}

		foreach(EnemyShot es in emd.shotF)
		{
			motion.shotF.AppendInterval(es.execTime - motion.shotF.Duration());
			motion.shotF.AppendCallback(() =>
			bm.CreateBullet(es.power, es.speed, transform.localPosition, es.angleDeg * Mathf.Deg2Rad));
		}

		// 逃げるシーケンスセット
		motion.esc.AppendInterval(emd.escTime);
		motion.esc.AppendCallback(() => EscapeFromPlayer());
		motion.esc.AppendInterval(3.0f);
		motion.esc.AppendCallback(() => Destroy(gameObject));

		// モーション実行
		motion.move.Play();
		motion.shotP.Play();
		motion.shotF.Play();
		motion.esc.Play();
	}
	
	// プレイヤーと反対方向に逃げて消える
	void EscapeFromPlayer()
	{
		Vector3 vec = (transform.localPosition - pt.localPosition).normalized;
		transform.DOLocalMove(vec * Screen.width, 3.0f).SetRelative().SetEase(curve);
	}

	void ShotToPlayer(float rot, float power, float speed)
	{
		if(pt)
		{
			float angle = Mathf.Atan2((pt.localPosition.x - transform.localPosition.x), (pt.localPosition.y - transform.localPosition.y));
			bm.CreateBullet(power, speed, transform.localPosition, angle + rot);
		}
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
				lifeImage.fillAmount = 0;
				Destroy(this.gameObject);

				// エフェクトでも出そう
			}
			else
			{
				lifeImage.DOFillAmount(life / maxLife, 0.5f);
			}
		}
	}
}

// モーションシーケンスクラス
class Motion
{
	public Sequence move	= DOTween.Sequence();
	public Sequence shotP	= DOTween.Sequence();
	public Sequence shotF	= DOTween.Sequence();
	public Sequence esc		= DOTween.Sequence();
}