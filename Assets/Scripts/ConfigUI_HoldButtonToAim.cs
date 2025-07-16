using UnityEngine;
using UnityEngine.UI;

public class ConfigUI_HoldButtonToAim : MonoBehaviour
{
	public Slider slider;

	private bool value
	{
		get
		{
			if (DataManager.data == null)
			{
				return false;
			}
			return DataManager.data.holdToAim;
		}
		set
		{
			if (DataManager.data != null)
			{
				DataManager.data.holdToAim = value;
			}
		}
	}

	private void Awake()
	{
		slider.onValueChanged.AddListener(OnSliderValueChange);
	}

	private void Start()
	{
		RefreshUI();
	}

	private void OnSliderValueChange(float value)
	{
		if (value > 0.5f)
		{
			this.value = true;
		}
		else
		{
			this.value = false;
		}
	}

	private void RefreshUI()
	{
		slider.value = (value ? 1 : 0);
	}

	public void SetValue(bool value)
	{
		this.value = value;
		RefreshUI();
	}

	public void Toggle()
	{
		SetValue(!value);
	}
}
