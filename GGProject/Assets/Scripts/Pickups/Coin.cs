using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Pickup
{
    protected override void PickUp()
    {
        base.PickUp();

        playerData.AddCoin();
    }
}
