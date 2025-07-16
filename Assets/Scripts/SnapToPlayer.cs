using UnityEngine;

public class SnapToPlayer : MonoBehaviour
{
	public bool snapToFirstPersonCameraTransform;

	private void Update()
	{
		PlayerControl player = LevelManager.instance.Player;
		if ((bool)player)
		{
			Transform transform = player.transform;
			if (snapToFirstPersonCameraTransform && player.sideCameraTransform != null)
			{
				transform = player.sideCameraTransform;
			}
			base.transform.position = transform.position;
			base.transform.rotation = transform.rotation;
		}
	}
}
