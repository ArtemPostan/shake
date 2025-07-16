using UnityEngine;

namespace FlamingCore
{
	public static class FCTool
	{
		public static Quaternion LookRotation2D(Vector2 lookDirection)
		{
			float num = 57.29578f * Mathf.Atan(lookDirection.y / lookDirection.x);
			num = ((!(lookDirection.x > 0f)) ? (num + 90f) : (num - 90f));
			return Quaternion.Euler(0f, 0f, num);
		}

		public static Vector3 Vector3YToZero(Vector3 v3)
		{
			return new Vector3(v3.x, 0f, v3.z);
		}
	}
}
