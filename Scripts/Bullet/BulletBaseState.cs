using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBaseState
{
    public abstract void EnterState(BulletStateManager bullet, Vector3 target);
    public abstract void UpdateState(BulletStateManager bullet);
    public abstract void OnTriggerEnter(BulletStateManager bullet, Collider other);
}

