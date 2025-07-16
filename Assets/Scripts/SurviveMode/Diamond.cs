using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : Receavable
{   
    public override void ItemUsage(Combat playerCombat)
    {
       playerCombat.UpdateEnergy();
    }
}
