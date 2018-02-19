using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
	[SerializeField]
	AnimationCurve curve;

	float           power = 0;
	float			life = 0;
	float			maxLife = 0;

	EnemyManager    em;
	BulletManager	bm;
	Player          pl;
	Transform		pt;
	CanvasGroup		cg;
	Image			lifeImage;
	
	Motion motion;		// モーション

	void Awake()
	{
		em = transform.parent.GetComponent<EnemyManager>();
		bm = GameObject.Find("BulletManager").GetComponent<BulletManager>();
		pl = GameObject.Find("Player") ? GameObject.Find("Player").GetComponent<Player>() : null;
		pt = GameObject.Find("Player") ? GameObject.Find("Player").transform : null;
		cg = gameObject.GetComponent<CanvasGroup>();
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
	public void Init(Vector3 pos, float p, float l, EnemyMotionData emd)
	{
		transform.localPosition = pos;
		cg.alpha = 0;

		// 動作セット
		SetMove(emd);

		// パワーセット
		power = p;

		// ライフセット
		life = l;
		maxLife = l;
	}

	void SetMove(EnemyMotionData emd)
	{
		// 出現シーケンスのセット
		switch(emd.popType)
		{
		case 0:	// フェードイン
			motion.pop.Append(cg.DOFade(1, 0.5f));
			break;
		case 1:
			motion.pop.AppendCallback(() => cg.alpha = 1);
			break;
		}

		// 移動シーケンスのセット
		foreach(EnemyMove em in emd.move)
		{
			motion.move.AppendInterval(em.execTime - motion.move.Duration());
			motion.move.Append(transform.DOLocalMove(em.move, em.flowTime)).SetRelative();
		}

		// 対プレイヤー弾シーケンスのセット
		foreach(EnemyShot es in emd.shotP)
		{
			motion.shotP.AppendInterval(es.execTime - motion.shotP.Duration());
			motion.shotP.AppendCallback(() => ShotToPlayer((es.angleDeg * Mathf.Deg2Rad), power, es.speed));
		}
		
		// フリー弾シーケンスのセット
		foreach(EnemyShot es in emd.shotF)
		{
			motion.shotF.AppendInterval(es.execTime - motion.shotF.Duration());
			motion.shotF.AppendCallback(() =>
			bm.CreateBullet(power, es.speed, transform.localPosition, es.angleDeg * Mathf.Deg2Rad));
		}

		// 逃げるシーケンスセット
		switch(emd.escType)
		{
		case 0:	// フェードアウト
			motion.esc.AppendInterval(emd.escTime);
			motion.esc.Append(cg.DOFade(0, 0.5f));
			motion.esc.AppendCallback(() => Destroy(gameObject));
			break;
		case 1:	// オプトアウト
			motion.esc.AppendInterval(emd.escTime);
			motion.esc.AppendCallback(() => EscapeFromPlayer());
			motion.esc.AppendInterval(3.0f);
			motion.esc.AppendCallback(() => Destroy(gameObject));
			break;
		}

		// モーション実行
		motion.pop.Play();
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

	// プレイヤーに向けて弾を撃つ
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
				em.BombEffect(transform.localPosition);
				if(pl) pl.DestroyEnemy();
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
	public Sequence pop		= DOTween.Sequence();	// 登場シーケンス
	public Sequence move	= DOTween.Sequence();	// 移動シーケンス
	public Sequence shotP	= DOTween.Sequence();	// 対プレイヤー弾シーケンス
	public Sequence shotF	= DOTween.Sequence();	// フリー弾シーケンス
	public Sequence esc		= DOTween.Sequence();	// 逃げるシーケンス
}