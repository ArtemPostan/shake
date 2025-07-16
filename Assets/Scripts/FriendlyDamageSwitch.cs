using UnityEngine;

public class FriendlyDamageSwitch : MonoBehaviour
{
	public struct FriendlyDamageSwitchEventContext
	{
		public bool friendlyDamageOn;
	}

	public delegate void FriendlyDamageSwitchEventHandler(FriendlyDamageSwitchEventContext context);

	private static bool _friendlyDamageOn = true;

	public Animator animator;

	public GameObject achievementDisabledNotification;

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

	public static bool FriendlyDamageOn => _friendlyDamageOn;

	public static event FriendlyDamageSwitchEventHandler onFriendlyDamageStatusChanged;

	public static void SetFriendlyDamage(bool value)
	{
		if (_friendlyDamageOn != value)
		{
			_friendlyDamageOn = value;
			FriendlyDamageSwitch.onFriendlyDamageStatusChanged?.Invoke(new FriendlyDamageSwitchEventContext
			{
				friendlyDamageOn = value
			});
		}
	}

	private void Awake()
	{
		onFriendlyDamageStatusChanged += OnFriendlyDamageStatusChanged;
	}

	private void OnDestroy()
	{
		onFriendlyDamageStatusChanged -= OnFriendlyDamageStatusChanged;
	}

	private void OnFriendlyDamageStatusChanged(FriendlyDamageSwitchEventContext context)
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
		SetFriendlyDamage(!FriendlyDamageOn);
		AudioManager.PlaySFXAtPosition(FriendlyDamageOn ? sfxTurnedOn : sfxTurnedOff, base.transform.position);
		AudioManager.PlaySFXAtPosition(sfxToggle, base.transform.position);
	}

	public void Refresh()
	{
		animatorTargetValue = (FriendlyDamageOn ? 1 : 0);
		if ((bool)achievementDisabledNotification)
		{
			achievementDisabledNotification.SetActive(!FriendlyDamageOn);
		}
		GameObject[] array = onObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(FriendlyDamageOn);
		}
		array = offObjects;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(!FriendlyDamageOn);
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
