using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D.Platformer
{
	[RequireComponent(typeof(SphereCollider))]
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerController : MonoBehaviour
	{
		public float PlayerSpeed = 5.5f;

		public MovementAxis Axis;

		private Vector3 _targetVelocity = Vector3.zero;

		private void FixedUpdate()
		{
			switch (Axis)
			{
			case MovementAxis.XY:
				_targetVelocity = new Vector3(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"), 0f);
				break;
			case MovementAxis.XZ:
				_targetVelocity = new Vector3(UnityEngine.Input.GetAxis("Horizontal"), 0f, UnityEngine.Input.GetAxis("Vertical"));
				break;
			case MovementAxis.YZ:
				_targetVelocity = new Vector3(0f, UnityEngine.Input.GetAxis("Vertical"), UnityEngine.Input.GetAxis("Horizontal"));
				break;
			}
			_targetVelocity *= PlayerSpeed;
			GetComponent<Rigidbody>().AddForce(_targetVelocity, ForceMode.Force);
		}
	}
}
