using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
	static SceneSwitcher _sceneSwitcher;
	public static SceneSwitcher SceneSwitch
	{
		get
		{
			return _sceneSwitcher;
		}
	}

	void Awake()
	{
		_sceneSwitcher = this;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			StartCoroutine(ChangeScene());
		}
	}

	public IEnumerator ChangeScene()
	{
		float fadeTime = SceneFaderUI.ScreenFader.BeginFade(SceneFaderUI.FadeDir.FadeIn);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene("Martin-BattleScene");
	}
}
