using UnityEngine;

public class Boom : MonoBehaviour
{
	public float radius;

	public float force;

	public GameObject follower;

	private void Start()
	{
	}

	private void Update()
	{
		if (CheatModeSwitch.CheatModeOn && UnityEngine.Input.GetKeyDown(KeyCode.LeftControl))
		{
			GameManager.Instance.BoomManager.Boom(PointerPoint() - Vector3.up * 0.3f, radius, force, 20, 3);
		}
		if (CheatModeSwitch.CheatModeOn && UnityEngine.Input.GetKeyDown(KeyCode.P))
		{
			Object.Instantiate(follower, PointerPoint() + Vector3.up, Quaternion.Euler(0f, 0f, 0f));
		}
	}

	private Vector3 PointerPoint()
	{
		if (GameManager.Instance.LevelManager.game3CType == LevelManager.game3Ctypes.fps)
		{
			return GameManager.Instance.LevelManager.Player.transform.position + GameManager.Instance.CameraManager.FpsCameraArm.transform.forward * 7f;
		}
		return base.transform.position;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.transform.position, radius);
	}
}
