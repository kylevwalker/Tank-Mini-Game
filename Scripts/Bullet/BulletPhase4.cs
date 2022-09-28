using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhase4 : BulletBaseState
{
    private Transform bulletTransform;
    private Vector3 targetPos;
    private float travelSpeed = 20f;
    public override void EnterState(BulletStateManager bullet, Vector3 target)
    {
        Debug.Log("Phase 4");
        bulletTransform = bullet.bulletTransform;
        targetPos = target;

    }
    public override void UpdateState(BulletStateManager bullet)
    {
        bulletTransform.position = Vector3.Lerp(bulletTransform.position, targetPos, travelSpeed * Time.deltaTime);
    }
    public override void OnTriggerEnter(BulletStateManager bullet, Collider other)
    {
        //Debug.Log("Hit with " + other);
    }
}
