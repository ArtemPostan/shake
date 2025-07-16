using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D
{
	[HelpURL("http://www.procamera2d.com/user-guide/extension-limit-distance/")]
	public class ProCamera2DLimitDistance : BasePC2D, IPositionDeltaChanger
	{
		public static string ExtensionName = "Limit Distance";

		public bool UseTargetsPosition = true;

		public bool LimitTopCameraDistance = true;

		[Range(0.1f, 1f)]
		public float MaxTopTargetDistance = 0.8f;

		public bool LimitBottomCameraDistance = true;

		[Range(0.1f, 1f)]
		public float MaxBottomTargetDistance = 0.8f;

		public bool LimitLeftCameraDistance = true;

		[Range(0.1f, 1f)]
		public float MaxLeftTargetDistance = 0.8f;

		public bool LimitRightCameraDistance = true;

		[Range(0.1f, 1f)]
		public float MaxRightTargetDistance = 0.8f;

		private int _pdcOrder = 2000;

		public int PDCOrder
		{
			get
			{
				return _pdcOrder;
			}
			set
			{
				_pdcOrder = value;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			base.ProCamera2D.AddPositionDeltaChanger(this);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			base.ProCamera2D.RemovePositionDeltaChanger(this);
		}

		public Vector3 AdjustDelta(float deltaTime, Vector3 originalDelta)
		{
			if (!base.enabled)
			{
				return originalDelta;
			}
			float num = Vector3H(originalDelta);
			bool flag = false;
			float num2 = Vector3V(originalDelta);
			bool flag2 = false;
			Vector2 vector = UseTargetsPosition ? new Vector2(Vector3H(base.ProCamera2D.TargetsMidPoint), Vector3V(base.ProCamera2D.TargetsMidPoint)) : new Vector2(Vector3H(base.ProCamera2D.CameraTargetPosition), Vector3V(base.ProCamera2D.CameraTargetPosition));
			if (LimitTopCameraDistance)
			{
				float num3 = base.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2f * MaxTopTargetDistance;
				if (vector.y > num2 + Vector3V(base.ProCamera2D.LocalPosition) + num3)
				{
					num2 = vector.y - (Vector3V(base.ProCamera2D.LocalPosition) + num3);
					flag2 = true;
				}
			}
			if (LimitBottomCameraDistance)
			{
				float num4 = base.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2f * MaxBottomTargetDistance;
				if (vector.y < num2 + Vector3V(base.ProCamera2D.LocalPosition) - num4)
				{
					num2 = vector.y - (Vector3V(base.ProCamera2D.LocalPosition) - num4);
					flag2 = true;
				}
			}
			if (LimitLeftCameraDistance)
			{
				float num5 = base.ProCamera2D.ScreenSizeInWorldCoordinates.x / 2f * MaxLeftTargetDistance;
				if (vector.x < num + Vector3H(base.ProCamera2D.LocalPosition) - num5)
				{
					num = vector.x - (Vector3H(base.ProCamera2D.LocalPosition) - num5);
					flag = true;
				}
			}
			if (LimitRightCameraDistance)
			{
				float num6 = base.ProCamera2D.ScreenSizeInWorldCoordinates.x / 2f * MaxRightTargetDistance;
				if (vector.x > num + Vector3H(base.ProCamera2D.LocalPosition) + num6)
				{
					num = vector.x - (Vector3H(base.ProCamera2D.LocalPosition) + num6);
					flag = true;
				}
			}
			base.ProCamera2D.CameraTargetPositionSmoothed = new Vector2(flag ? (Vector3H(base.ProCamera2D.LocalPosition) + num) : Vector3H(base.ProCamera2D.CameraTargetPositionSmoothed), flag2 ? (Vector3V(base.ProCamera2D.LocalPosition) + num2) : Vector3V(base.ProCamera2D.CameraTargetPositionSmoothed));
			return VectorHV(num, num2);
		}
	}
}
