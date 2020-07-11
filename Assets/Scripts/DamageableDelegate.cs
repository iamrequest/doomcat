using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableDelegate : Damageable {
    public Damageable parent;


    public override void TakeDamage(int damage) {
        if (parent.isInvincible) return;

        base.TakeDamage(damage);
        parent.TakeDamage(damage);
    }
}
