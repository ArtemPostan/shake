using UnityEngine;

public class RenderFactorConfiguration : MonoBehaviour
{
	public SliderConfiguration slider;

	private void Awake()
	{
		if (slider == null)
		{
			slider = GetComponent<SliderConfiguration>();
		}
		slider.onApplyValue += OnApplyValue;
	}

	private void OnApplyValue(float value)
	{
		QualitySettings.resolutionScalingFixedDPIFactor = value;
	}
}
