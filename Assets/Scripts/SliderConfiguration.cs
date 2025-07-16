using UnityEngine;
using UnityEngine.UI;

public class SliderConfiguration : MonoBehaviour
{
	public delegate void SliderConfigurationEventHandler(float value);

	public InputField inputField;

	public Slider slider;

	public string playerPrefsKey = "config_RenderingFactor";

	public float value;

	public float minValue;

	public float maxValue = 1f;

	public float defaultValue = 1f;

	public event SliderConfigurationEventHandler onApplyValue;

	private void Awake()
	{
		slider.onValueChanged.AddListener(OnSliderValueChanged);
		inputField.onEndEdit.AddListener(OnInputFieldChanged);
	}

	private void Start()
	{
		ReadValue();
		RefreshUI();
	}

	private void RefreshUI()
	{
		RefreshSlider();
		RefreshTextField();
		ApplyValue();
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
		float num = this.value = Mathf.Lerp(minValue, maxValue, value);
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

	private void ApplyValue()
	{
		PlayerPrefs.SetFloat(playerPrefsKey, value);
		this.onApplyValue?.Invoke(value);
	}

	private void ReadValue()
	{
		if (PlayerPrefs.HasKey(playerPrefsKey))
		{
			value = PlayerPrefs.GetFloat(playerPrefsKey);
		}
		else
		{
			value = defaultValue;
		}
	}
}
