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

	/// <summary>
	/// Use StartCoroutine(ChangeScene()) to call, not just method name.
	/// </summary>
	/// <returns></returns>
	public IEnumerator ChangeScene(string sceneName)
	{
		float fadeTime = SceneFaderUI.ScreenFader.BeginFade(SceneFaderUI.FadeDir.FadeIn);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(sceneName);
	}
}
