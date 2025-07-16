using UnityEngine;

namespace ES3Internal
{
	public class ES3GlobalReferences : ScriptableObject
	{
		public static ES3GlobalReferences Instance => null;

		public UnityEngine.Object Get(long id)
		{
			return null;
		}

		public long GetOrAdd(UnityEngine.Object obj)
		{
			return -1L;
		}

		public void RemoveInvalidKeys()
		{
		}
	}
}
