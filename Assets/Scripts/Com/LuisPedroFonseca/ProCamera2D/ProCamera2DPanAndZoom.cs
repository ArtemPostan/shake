using UnityEngine;
using UnityEngine.EventSystems;

namespace Com.LuisPedroFonseca.ProCamera2D
{
	[HelpURL("http://www.procamera2d.com/user-guide/extension-pan-and-zoom/")]
	public class ProCamera2DPanAndZoom : BasePC2D, IPreMover
	{
		public enum MouseButton
		{
			Left,
			Right,
			Middle
		}

		public static string ExtensionName = "Pan And Zoom";

		public bool DisableOverUGUI = true;

		public bool AllowZoom = true;

		public float MouseZoomSpeed = 10f;

		public float PinchZoomSpeed = 50f;

		[Range(0f, 2f)]
		public float ZoomSmoothness = 0.2f;

		public float MaxZoomInAmount = 2f;

		public float MaxZoomOutAmount = 2f;

		public bool ZoomToInputCenter = true;

		private float _zoomAmount;

		private float _initialCamSize;

		private bool _zoomStarted;

		private float _origFollowSmoothnessX;

		private float _origFollowSmoothnessY;

		private float _prevZoomAmount;

		private float _zoomVelocity;

		private Vector3 _zoomPoint;

		private float _touchZoomTime;

		public bool AllowPan = true;

		public bool UsePanByDrag = true;

		[Range(0f, 1f)]
		public float StopSpeedOnDragStart = 0.95f;

		public Rect DraggableAreaRect = new Rect(0f, 0f, 1f, 1f);

		public Vector2 DragPanSpeedMultiplier = new Vector2(1f, 1f);

		public bool UsePanByMoveToEdges;

		public Vector2 EdgesPanSpeed = new Vector2(2f, 2f);

		[Range(0f, 0.99f)]
		public float TopPanEdge = 0.9f;

		[Range(0f, 0.99f)]
		public float BottomPanEdge = 0.9f;

		[Range(0f, 0.99f)]
		public float LeftPanEdge = 0.9f;

		[Range(0f, 0.99f)]
		public float RightPanEdge = 0.9f;

		public MouseButton PanMouseButton;

		[HideInInspector]
		public bool ResetPrevPanPoint;

		private Vector2 _panDelta;

		private Transform _panTarget;

		private Vector3 _prevMousePosition;

		private Vector3 _prevTouchPosition;

		private int _prevTouchId;

		private bool _onMaxZoom;

		private bool _onMinZoom;

		private EventSystem _eventSystem;

		private bool _skip;

		private int _prmOrder;

		public int PrMOrder
		{
			get
			{
				return _prmOrder;
			}
			set
			{
				_prmOrder = value;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			UpdateCurrentFollowSmoothness();
			_eventSystem = EventSystem.current;
			_panTarget = new GameObject("PC2DPanTarget").transform;
			base.ProCamera2D.AddPreMover(this);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			base.ProCamera2D.RemovePreMover(this);
		}

		private void Start()
		{
			_initialCamSize = base.ProCamera2D.ScreenSizeInWorldCoordinates.y * 0.5f;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			CenterPanTargetOnCamera();
			base.ProCamera2D.AddCameraTarget(_panTarget);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			ResetPrevPanPoint = true;
			_onMaxZoom = false;
			_onMinZoom = false;
			base.ProCamera2D.RemoveCameraTarget(_panTarget);
		}

		public void PreMove(float deltaTime)
		{
			_skip = (DisableOverUGUI && (bool)_eventSystem && _eventSystem.IsPointerOverGameObject());
			if (_skip)
			{
				_prevMousePosition = new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, Mathf.Abs(Vector3D(base.ProCamera2D.LocalPosition)));
				CancelZoom();
			}
			if (base.enabled && AllowPan && !_skip)
			{
				Pan(deltaTime);
			}
			if (base.enabled && AllowZoom && !_skip)
			{
				Zoom(deltaTime);
			}
		}

		private void Pan(float deltaTime)
		{
			_panDelta = Vector2.zero;
			Vector2 vector = DragPanSpeedMultiplier;
			if (UsePanByDrag && Input.GetMouseButtonDown((int)PanMouseButton))
			{
				CenterPanTargetOnCamera(StopSpeedOnDragStart);
			}
			Vector3 vector2 = new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, Mathf.Abs(Vector3D(base.ProCamera2D.LocalPosition)));
			if (UsePanByDrag && Input.GetMouseButton((int)PanMouseButton))
			{
				Vector2 normalizedInput = new Vector2(UnityEngine.Input.mousePosition.x / (float)base.ProCamera2D.GameCamera.pixelWidth, UnityEngine.Input.mousePosition.y / (float)base.ProCamera2D.GameCamera.pixelHeight);
				if (base.ProCamera2D.GameCamera.pixelRect.Contains(vector2) && InsideDraggableArea(normalizedInput))
				{
					Vector3 a = base.ProCamera2D.GameCamera.ScreenToWorldPoint(_prevMousePosition);
					if (ResetPrevPanPoint)
					{
						a = base.ProCamera2D.GameCamera.ScreenToWorldPoint(vector2);
						ResetPrevPanPoint = false;
					}
					Vector3 b = base.ProCamera2D.GameCamera.ScreenToWorldPoint(vector2);
					Vector3 arg = a - b;
					_panDelta = new Vector2(Vector3H(arg), Vector3V(arg));
				}
			}
			else if (UsePanByMoveToEdges && !Input.GetMouseButton((int)PanMouseButton))
			{
				float num = ((float)(-Screen.width) * 0.5f + UnityEngine.Input.mousePosition.x) / (float)Screen.width;
				float num2 = ((float)(-Screen.height) * 0.5f + UnityEngine.Input.mousePosition.y) / (float)Screen.height;
				if (num < 0f)
				{
					num = num.Remap(-0.5f, (0f - LeftPanEdge) * 0.5f, -0.5f, 0f);
				}
				else if (num > 0f)
				{
					num = num.Remap(RightPanEdge * 0.5f, 0.5f, 0f, 0.5f);
				}
				if (num2 < 0f)
				{
					num2 = num2.Remap(-0.5f, (0f - BottomPanEdge) * 0.5f, -0.5f, 0f);
				}
				else if (num2 > 0f)
				{
					num2 = num2.Remap(TopPanEdge * 0.5f, 0.5f, 0f, 0.5f);
				}
				_panDelta = new Vector2(num, num2) * deltaTime;
				if (_panDelta != Vector2.zero)
				{
					vector = EdgesPanSpeed;
				}
			}
			_prevMousePosition = vector2;
			if (_panDelta != Vector2.zero)
			{
				Vector3 translation = VectorHV(_panDelta.x * vector.x, _panDelta.y * vector.y);
				_panTarget.Translate(translation);
			}
			if ((base.ProCamera2D.IsCameraPositionLeftBounded && Vector3H(_panTarget.position) < Vector3H(base.ProCamera2D.LocalPosition)) || (base.ProCamera2D.IsCameraPositionRightBounded && Vector3H(_panTarget.position) > Vector3H(base.ProCamera2D.LocalPosition)))
			{
				_panTarget.position = VectorHVD(Vector3H(base.ProCamera2D.LocalPosition), Vector3V(_panTarget.position), Vector3D(_panTarget.position));
			}
			if ((base.ProCamera2D.IsCameraPositionBottomBounded && Vector3V(_panTarget.position) < Vector3V(base.ProCamera2D.LocalPosition)) || (base.ProCamera2D.IsCameraPositionTopBounded && Vector3V(_panTarget.position) > Vector3V(base.ProCamera2D.LocalPosition)))
			{
				_panTarget.position = VectorHVD(Vector3H(_panTarget.position), Vector3V(base.ProCamera2D.LocalPosition), Vector3D(_panTarget.position));
			}
		}

		private void Zoom(float deltaTime)
		{
			float num = 0f;
			num = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
			_zoomPoint = new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, Mathf.Abs(Vector3D(base.ProCamera2D.LocalPosition)));
			if (!base.ProCamera2D.GameCamera.pixelRect.Contains(_zoomPoint))
			{
				return;
			}
			float num2 = 0f;
			num2 = MouseZoomSpeed;
			if ((_onMaxZoom && num * num2 < 0f) || (_onMinZoom && num * num2 > 0f))
			{
				CancelZoom();
				return;
			}
			_zoomAmount = Mathf.SmoothDamp(_prevZoomAmount, num * num2 * deltaTime, ref _zoomVelocity, ZoomSmoothness, float.MaxValue, deltaTime);
			if (Mathf.Abs(_zoomAmount) <= 0.0001f)
			{
				if (_zoomStarted)
				{
					RestoreFollowSmoothness();
				}
				_zoomStarted = false;
				_prevZoomAmount = 0f;
				return;
			}
			if (!_zoomStarted)
			{
				_zoomStarted = true;
				_panTarget.position = base.ProCamera2D.LocalPosition - base.ProCamera2D.InfluencesSum;
				UpdateCurrentFollowSmoothness();
				RemoveFollowSmoothness();
			}
			float num3 = base.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2f + _zoomAmount;
			float num4 = _initialCamSize / MaxZoomInAmount;
			float num5 = MaxZoomOutAmount * _initialCamSize;
			_onMaxZoom = false;
			_onMinZoom = false;
			if (num3 < num4)
			{
				_zoomAmount -= num3 - num4;
				_onMaxZoom = true;
			}
			else if (num3 > num5)
			{
				_zoomAmount -= num3 - num5;
				_onMinZoom = true;
			}
			_prevZoomAmount = _zoomAmount;
			if (ZoomToInputCenter && _zoomAmount != 0f)
			{
				float d = _zoomAmount / (base.ProCamera2D.ScreenSizeInWorldCoordinates.y / 2f);
				_panTarget.position += (_panTarget.position - base.ProCamera2D.GameCamera.ScreenToWorldPoint(_zoomPoint)) * d;
			}
			base.ProCamera2D.Zoom(_zoomAmount);
		}

		public void UpdateCurrentFollowSmoothness()
		{
			_origFollowSmoothnessX = base.ProCamera2D.HorizontalFollowSmoothness;
			_origFollowSmoothnessY = base.ProCamera2D.VerticalFollowSmoothness;
		}

		public void CenterPanTargetOnCamera(float interpolant = 1f)
		{
			if (_panTarget != null)
			{
				_panTarget.position = Vector3.Lerp(_panTarget.position, VectorHV(Vector3H(base.ProCamera2D.LocalPosition), Vector3V(base.ProCamera2D.LocalPosition)), interpolant);
			}
		}

		private void CancelZoom()
		{
			_zoomAmount = 0f;
			_prevZoomAmount = 0f;
			_zoomVelocity = 0f;
		}

		private void RestoreFollowSmoothness()
		{
			base.ProCamera2D.HorizontalFollowSmoothness = _origFollowSmoothnessX;
			base.ProCamera2D.VerticalFollowSmoothness = _origFollowSmoothnessY;
		}

		private void RemoveFollowSmoothness()
		{
			base.ProCamera2D.HorizontalFollowSmoothness = 0f;
			base.ProCamera2D.VerticalFollowSmoothness = 0f;
		}

		private bool InsideDraggableArea(Vector2 normalizedInput)
		{
			if (DraggableAreaRect.x == 0f && DraggableAreaRect.y == 0f && DraggableAreaRect.width == 1f && DraggableAreaRect.height == 1f)
			{
				return true;
			}
			if (normalizedInput.x > DraggableAreaRect.x + (1f - DraggableAreaRect.width) / 2f && normalizedInput.x < DraggableAreaRect.x + DraggableAreaRect.width + (1f - DraggableAreaRect.width) / 2f && normalizedInput.y > DraggableAreaRect.y + (1f - DraggableAreaRect.height) / 2f && normalizedInput.y < DraggableAreaRect.y + DraggableAreaRect.height + (1f - DraggableAreaRect.height) / 2f)
			{
				return true;
			}
			return false;
		}
	}
}
