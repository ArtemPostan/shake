using UnityEngine;
using UnityEngine.UI;

public class MixerConfiguration : MonoBehaviour
{
	public enum Type
	{
		Master,
		SFX,
		BGM
	}

	public Type type;

	public InputField inputField;

	public Slider slider;

	public float minValue = -80f;

	public float maxValue = 12f;

	public float remapMin;

	public float remapMax = 100f;

	public float value
	{
		get
		{
			if (!AudioManager.instance)
			{
				return 0f;
			}
			switch (type)
			{
			case Type.Master:
				return AudioManager.instance.GetPreferredMasterVolume();
			case Type.SFX:
				return AudioManager.instance.GetPreferredSFXVolume();
			case Type.BGM:
				return AudioManager.instance.GetPreferredBGMVolume();
			default:
				return 0f;
			}
		}
		set
		{
			switch (type)
			{
			case Type.Master:
				AudioManager.instance.SetPreferredMasterVolume(value);
				break;
			case Type.SFX:
				AudioManager.instance.SetPreferredSFXVolume(value);
				break;
			case Type.BGM:
				AudioManager.instance.SetPreferredBGMVolume(value);
				break;
			}
			AudioManager.RefreshValues();
		}
	}

	private void Awake()
	{
		slider.onValueChanged.AddListener(OnSliderValueChanged);
		inputField.onEndEdit.AddListener(OnInputFieldChanged);
	}

	private void Start()
	{
		RefreshUI();
	}

	private void RefreshUI()
	{
		RefreshSlider();
		RefreshTextField();
	}

	private void RefreshTextField()
	{
		inputField.text = ValueToDisplay(value).ToString("##0");
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
			this.value = DisplayToValue(result);
		}
		RefreshUI();
	}

	private float ValueToDisplay(float value)
	{
		return Mathf.Lerp(remapMin, remapMax, (value - minValue) / (maxValue - minValue));
	}

	private float DisplayToValue(float display)
	{
		return Mathf.Lerp(minValue, maxValue, (display - remapMin) / (remapMax - remapMin));
	}
}
