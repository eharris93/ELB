using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public static class SceneSwitcher
{
	/// <summary>
	/// Use StartCoroutine(ChangeScene()) to call, not just method name.
	/// </summary>
	/// <returns></returns>
	public static IEnumerator ChangeScene(string sceneName)
	{
		SceneFaderUI.ScreenFader.StartFadeOverTime(SceneFaderUI.FadeDir.FadeIn);
		yield return new WaitForSeconds(SceneFaderUI.ScreenFader.FadeTime);
		SceneManager.LoadScene(sceneName);
	}

	public static IEnumerator ChangeScene(string sceneName, LoadSceneMode loadSceneMode)
	{
		SceneFaderUI.ScreenFader.StartFadeOverTime(SceneFaderUI.FadeDir.FadeIn);
		yield return new WaitForSeconds(SceneFaderUI.ScreenFader.FadeTime);
		SceneManager.LoadScene(sceneName, loadSceneMode);
	}

	public static IEnumerator ChangeScene(int sceneBuildIndex)
	{
		SceneFaderUI.ScreenFader.StartFadeOverTime(SceneFaderUI.FadeDir.FadeIn);
		yield return new WaitForSeconds(SceneFaderUI.ScreenFader.FadeTime);
		SceneManager.LoadScene(sceneBuildIndex);
	}

	public static IEnumerator ChangeScene(int sceneBuildIndex, LoadSceneMode loadSceneMode)
	{
		SceneFaderUI.ScreenFader.StartFadeOverTime(SceneFaderUI.FadeDir.FadeIn);
		yield return new WaitForSeconds(SceneFaderUI.ScreenFader.FadeTime);
		SceneManager.LoadScene(sceneBuildIndex, loadSceneMode);
	}
}
