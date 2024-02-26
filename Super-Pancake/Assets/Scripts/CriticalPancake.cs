using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lukas.Utilities;

public class CriticalPancake : Pancake
{

    public override void butterExplosion()
    {
        freefall = false;

        exploding = true;

        myExplosionRadius.enabled = true;

        gameObject.tag = "Explosion";

        EffectsManager.instance.ActivateScreenShake(1f, 1.1f);

        Debug.Log("You got the critical flip! You win!");
    }

}
