using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePickup : Pickup
{
    protected override void PickUp()
    {
        base.PickUp();

        playerMovement.grappleEnabled = true;
    }
}
