using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 画面切り替えやフェード、ロードなどを管理するクラス（になる予定）
/// </summary>
public class ScreenManager : SingletonMonoBehaviour<ScreenManager> {

	private bool loadComp;

	void Awake()
	{
		//とりあえずDontDestroyで
		if (base.CheckInstance ()) {
			DontDestroyOnLoad (this.gameObject);
		}
	}

	void Update()
	{
        //終了処理
        if( Input.GetKeyDown(KeyCode.C) && Input.GetKeyDown(KeyCode.R) && Input.GetKeyDown(KeyCode.I))
        {
            Application.Quit();
        }
		//TestCode -- あとでいじる人消しといて
		/*if (Input.GetKeyDown (KeyCode.P)) {
			LoadSceneAsync ("NoteEditor",test);
		}*/
	}

    //実験用　消して
    /*private void test()
    {
        Debug.Log("終了！");
    }*/

	public void LoadScene(string sceneName)
	{
		
		SceneManager.LoadScene (sceneName);
	}

	public void LoadSceneAsync(string sceneName , System.Action onFinished)
	{
		StartCoroutine (loadAsync(sceneName,onFinished));

	}

   
	private IEnumerator loadAsync(string sceneName , System.Action onFinished)
	{
		loadComp = false;
		AsyncOperation ope = SceneManager.LoadSceneAsync (sceneName);
		ope.allowSceneActivation = false;
		while (ope.progress >= 0.9) {
			yield return null;
		}
		//ロード画面の雰囲気は大事なので読み込み終わってても１秒待たせる
		yield return new WaitForSeconds(1);
		ope.allowSceneActivation = true;
		loadComp = true;
        onFinished();
	}
}
