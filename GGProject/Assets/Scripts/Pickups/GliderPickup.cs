using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GliderPickup : Pickup
{
    protected override void PickUp()
    {
        base.PickUp();

        playerMovement.glideEnabled = true;
    }
}
