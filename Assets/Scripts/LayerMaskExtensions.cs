using UnityEngine;

public static class LayerMaskExtensions
{
	public static bool Contains(this LayerMask layerMask, int layer)
	{
		return ((int)layerMask | (1 << layer)) == (int)layerMask;
	}
}
