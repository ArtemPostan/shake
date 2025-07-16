using UnityEngine;

public abstract class Receavable : MonoBehaviour
{
    Combat playerCombat;
    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent<PlayerControl>(out var player))
        {
            playerCombat = player.GetComponent<Combat>();
            ItemUsage(playerCombat);
            Destroy(gameObject);
        }
    }
    public abstract void ItemUsage(Combat playerCombat);
   
}