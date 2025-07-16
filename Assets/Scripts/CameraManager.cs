using UnityEngine;

public class CameraManager : MonoBehaviour
{
	[SerializeField]
	private TopDownCameraArm TopDownCameraArmPfb;

	private TopDownCameraArm topDownCameraArm;

	public Vector2 cameraTargetOffset;

	[SerializeField]
	private RenderCamera renderCameraPfb;

	private RenderCamera renderCamera;

	private FpsCameraArm fpsCameraArm;

	public TopDownCameraArm TopDownCameraArm => topDownCameraArm;

	public RenderCamera RenderCamera => renderCamera;

	public FpsCameraArm FpsCameraArm => fpsCameraArm;

	public void Init(PlayerControl player, Transform pointerTransform)
	{
		topDownCameraArm = UnityEngine.Object.Instantiate(TopDownCameraArmPfb);
		topDownCameraArm.ProCamera2d.AddCameraTarget(player.transform, 1f, 1f, 0f, cameraTargetOffset);
		topDownCameraArm.ProCamera2d.AddCameraTarget(pointerTransform, 0.35f, 0.35f, 0f, cameraTargetOffset);
		renderCamera = UnityEngine.Object.Instantiate(renderCameraPfb);
		renderCamera.SetTarget(topDownCameraArm.VirtualCamera);
		renderCamera.FadeIn();
		GameManager.Instance.UIManager.Init(renderCamera.TopCamera.Camera);
	}

	private void Awake()
	{
	}

	private void Update()
	{
		if (!fpsCameraArm || !topDownCameraArm)
		{
			return;
		}
		switch (GameManager.Instance.LevelManager.game3CType)
		{
		case LevelManager.game3Ctypes.topDown:
			renderCamera.SetTarget(topDownCameraArm.VirtualCamera);
			break;
		case LevelManager.game3Ctypes.fps:
			if (GameManager.Instance.LevelManager.GameState != LevelManager.gameStates.defeat)
			{
				renderCamera.SetTarget(fpsCameraArm.VirtualCamera);
			}
			else
			{
				renderCamera.SetTarget(topDownCameraArm.VirtualCamera);
			}
			break;
		}
	}

	public void SetFpsCamera(FpsCameraArm _fpsCameraArm)
	{
		fpsCameraArm = _fpsCameraArm;
	}
}
