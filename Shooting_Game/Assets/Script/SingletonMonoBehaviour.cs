using System.Collections;
using UnityEngine;

/// <summary>
/// MonoBehaviourのシングルトン対応クラス
/// </summary>
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T> 
{
	protected static T instance;

/// <summary>
///インスタンスのゲッター 
/// </summary>
/// <returns>クラスインスタンス</returns>
	public static T Instance
	{
		get{
			if(instance == null)
			{
				instance = (T)FindObjectOfType(typeof(T));
				if( instance == null)
				{
					//基本的にここは通らないはず
					Debug.LogWarning(typeof(T)+"は存在しません");
				}
			}
			return instance;
		}
	}

	protected void Awake()
	{
		CheckInstance();
	}

/// <summary>
/// インスタンスが存在するか確認し、存在しない場合は自身のインスタンスを使用する
/// 存在してる場合は自身のオブジェクトを破棄する
/// </summary>
/// <returns>自身を使用する場合true</returns>
	protected bool CheckInstance()
	{
		if( instance == null)
		{
			instance = (T)this;
			return true;
		}else if (Instance == this)
		{
			return true;
		}

		Destroy(this);
		return false;
	}

}
