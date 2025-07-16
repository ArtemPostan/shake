using Steamworks;
using UnityEngine;

public class JumpToSodaCrisis : MonoBehaviour
{
	public LayerMask visibleLayers;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnTriggerEnter(Collider other)
	{
		//if (visibleLayers.Contains(other.gameObject.layer) && SteamManager.Initialized)
		//{
		//	SteamFriends.ActivateGameOverlayToStore((AppId_t)1592670u, EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
		//}
	}
}
