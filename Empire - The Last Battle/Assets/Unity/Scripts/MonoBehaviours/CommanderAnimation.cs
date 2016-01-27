using UnityEngine;
using System.Collections;

public class CommanderAnimation : MonoBehaviour
{
	public CommanderUI CommanderUI;

	public float Angle;
	public float Period;

	float _time = 0f;

	void Start()
	{
		CommanderUI = GetComponentInChildren<CommanderUI>();
		CommanderUI.OnDraggingCommander += Animate;
		CommanderUI.OnCommanderGrounded += Reset;
	}

	void Animate()
	{
		_time = _time + Time.deltaTime;
		float phase = Mathf.Sin(_time / Period);
		transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, phase * Angle));
	}

	void Reset()
	{
		transform.rotation.Set(0.0f, 0.0f, 0.0f, 0.0f);
	}
}
