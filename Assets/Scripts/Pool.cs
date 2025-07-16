using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pool<T> where T : PooledBehaviour
{
	public Transform parent;

	public T prefab;

	private List<T> _inactiveList;

	private List<T> _activeList;

	[HideInInspector]
	private List<T> inactiveList
	{
		get
		{
			if (_inactiveList == null)
			{
				_inactiveList = new List<T>();
			}
			return _inactiveList;
		}
	}

	[HideInInspector]
	private List<T> activeList
	{
		get
		{
			if (_activeList == null)
			{
				_activeList = new List<T>();
			}
			return _activeList;
		}
	}

	public T GetOrCreate()
	{
		if (inactiveList.Count > 0)
		{
			T val = inactiveList[0];
			inactiveList.RemoveAt(0);
			val.Activate();
			activeList.Add(val);
			return val;
		}
		T val2 = UnityEngine.Object.Instantiate(prefab, parent);
		activeList.Add(val2);
		return val2;
	}

	public void Recycle(T entity)
	{
		if ((bool)(UnityEngine.Object)entity)
		{
			entity.Deactivate();
			entity.Reset();
			if (activeList.Contains(entity))
			{
				activeList.Remove(entity);
			}
			inactiveList.Add(entity);
		}
	}
}
