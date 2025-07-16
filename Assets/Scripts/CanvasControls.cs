using UnityEngine;
using UnityEngine.UI;

public class CanvasControls : MonoBehaviour
{
	public GameObject propeller;

	public Slider rpm;

	public Slider shutterSpeed;

	public Slider samples;

	public Slider alphaOffset;

	public Slider angularVelocityCutoff;

	public SimpleSpinBlur simpleSpinBlur;

	private void Start()
	{
	}

	private void Update()
	{
		propeller.transform.Rotate(new Vector3(rpm.value, 0f, 0f) * Time.deltaTime);
		simpleSpinBlur.shutterSpeed = (int)shutterSpeed.value;
		simpleSpinBlur.Samples = (int)samples.value;
		simpleSpinBlur.alphaOffset = alphaOffset.value;
		simpleSpinBlur.advancedSettings.AngularVelocityCutoff = angularVelocityCutoff.value;
	}
}
