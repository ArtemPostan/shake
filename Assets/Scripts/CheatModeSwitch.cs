using UnityEngine;

public class CheatModeSwitch : MonoBehaviour
{
	public struct CheatModeSwitchEventContext
	{
		public bool cheatModeOn;
	}

	public delegate void CheatModeSwitchEventHandler(CheatModeSwitchEventContext context);

	private static bool _cheatModeOn;

	public Animator animator;

	public GameObject[] onObjects;

	public GameObject[] offObjects;

	public LayerMask visibleLayers = -1;

	public float animationAcceleration = 10f;

	public float animationCollisionDamping = 0.5f;

	public string sfxToggle = "Toggle";

	public string sfxTurnedOn = "On";

	public string sfxTurnedOff = "Off";

	public float leverCollideSfxSpeedThreshold = 2f;

	public string sfxLeverCollision = "LeverCollide";

	[Range(0f, 1f)]
	public float animatorValue;

	private float animatorVelocity;

	[Range(0f, 1f)]
	public float animatorTargetValue;

	public static bool CheatModeOn => _cheatModeOn;

	public static event CheatModeSwitchEventHandler onCheatModeChanged;

	public static void SetCheatMode(bool value)
	{
		if (_cheatModeOn != value)
		{
			_cheatModeOn = value;
			CheatModeSwitch.onCheatModeChanged?.Invoke(new CheatModeSwitchEventContext
			{
				cheatModeOn = value
			});
		}
	}

	private void Awake()
	{
		onCheatModeChanged += OnCheatModeStatusChanged;
	}

	private void OnDestroy()
	{
		onCheatModeChanged -= OnCheatModeStatusChanged;
	}

	private void OnCheatModeStatusChanged(CheatModeSwitchEventContext context)
	{
		Refresh();
	}

	private void Start()
	{
		Refresh();
	}

	private void Update()
	{
		HandleAnimation();
	}

	private void HandleAnimation()
	{
		float num = animationAcceleration * (float)((animatorTargetValue > 0f) ? 1 : (-1));
		animatorVelocity += num * Time.deltaTime;
		animatorValue += animatorVelocity * Time.deltaTime;
		if (animatorValue > 1f)
		{
			animatorValue = 1f;
			if (animatorVelocity > 0f)
			{
				animatorVelocity = (0f - animatorVelocity) * animationCollisionDamping;
				if (Mathf.Abs(animatorVelocity) > leverCollideSfxSpeedThreshold)
				{
					AudioManager.PlaySFXAtPosition(sfxLeverCollision, base.transform.position);
				}
			}
		}
		else if (animatorValue < 0f)
		{
			animatorValue = 0f;
			if (animatorVelocity < 0f)
			{
				animatorVelocity = (0f - animatorVelocity) * animationCollisionDamping;
				if (Mathf.Abs(animatorVelocity) > leverCollideSfxSpeedThreshold)
				{
					AudioManager.PlaySFXAtPosition(sfxLeverCollision, base.transform.position);
				}
			}
		}
		animator.SetFloat("On", animatorValue);
	}

	public void Toggle()
	{
		SetCheatMode(!CheatModeOn);
		AudioManager.PlaySFXAtPosition(CheatModeOn ? sfxTurnedOn : sfxTurnedOff, base.transform.position);
		AudioManager.PlaySFXAtPosition(sfxToggle, base.transform.position);
	}

	public void Refresh()
	{
		animatorTargetValue = (CheatModeOn ? 1 : 0);
		GameObject[] array = onObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(CheatModeOn);
		}
		array = offObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(!CheatModeOn);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (visibleLayers.Contains(collision.gameObject.layer))
		{
			Toggle();
		}
	}
}
