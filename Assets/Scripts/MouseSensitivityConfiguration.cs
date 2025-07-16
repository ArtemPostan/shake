using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivityConfiguration : MonoBehaviour
{
	public InputField inputField;

	public Slider slider;

	[Min(0f)]
	public float minValue;

	[Min(0f)]
	public float maxValue = 100f;

	public float value
	{
		get
		{
			return DataManager.data.mouseSensitivity;
		}
		set
		{
			DataManager.data.mouseSensitivity = value;
		}
	}

	private void Awake()
	{
		value = DataManager.data.mouseSensitivity;
		RefreshUI();
		slider.onValueChanged.AddListener(OnSliderValueChanged);
		inputField.onEndEdit.AddListener(OnInputFieldChanged);
	}

	private void RefreshUI()
	{
		RefreshSlider();
		RefreshTextField();
	}

	private void RefreshTextField()
	{
		inputField.text = value.ToString("##0.00");
	}

	private void RefreshSlider()
	{
		slider.value = (value - minValue) / (maxValue - minValue);
	}

	private void OnSliderValueChanged(float value)
	{
		float num2 = this.value = Mathf.Lerp(minValue, maxValue, value);
		RefreshUI();
	}

	private void OnInputFieldChanged(string value)
	{
		if (float.TryParse(value, out float result))
		{
			this.value = result;
		}
		RefreshUI();
	}
}
