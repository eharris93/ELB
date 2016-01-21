using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneFaderUI : MonoBehaviour
{
	public enum FadeDir
	{
		FadeIn,
		FadeOut
	}

	public RawImage FadeOutTexture;
	public float FadeSpeed;

	private float _alpha = 1.0f;
	private int _fadeDir = -1;

	static SceneFaderUI _screenFader;
	public static SceneFaderUI ScreenFader
	{
		get
		{
			return _screenFader;
		}
	}

	void Awake()
	{
		_screenFader = this;
	}

	void OnGUI()
	{
		_alpha += _fadeDir * FadeSpeed * Time.deltaTime;
		_alpha = Mathf.Clamp01(_alpha);

		FadeOutTexture.color = new Color(FadeOutTexture.color.r, FadeOutTexture.color.g, FadeOutTexture.color.b, _alpha);
	}

	public float BeginFade(FadeDir fadeDir)
	{
		int direction;

		if (fadeDir == FadeDir.FadeIn)
		{
			direction = 1;
		}
		else
		{
			direction = -1;
		}

		_fadeDir = direction;
		return FadeSpeed;
	}

	void OnLevelWasLoaded()
	{
		BeginFade(FadeDir.FadeOut);
	}
}
