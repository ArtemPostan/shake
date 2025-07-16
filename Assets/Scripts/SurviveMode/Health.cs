using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Receavable
{
    public override void ItemUsage(Combat playerCombat)
    {
        playerCombat.UpdateHealth();
    }
}
