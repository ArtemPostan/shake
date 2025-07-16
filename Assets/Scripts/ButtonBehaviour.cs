using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonBehaviour : MonoBehaviour
{
	protected Button button;

	private void OnEnable()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(OnClick);
	}

	private void OnDisable()
	{
		button.onClick.RemoveListener(OnClick);
	}

	protected virtual void OnClick()
	{
	}
}
