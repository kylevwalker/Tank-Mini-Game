using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhase3 : BulletBaseState
{
    private Transform bulletTransform;
    private Vector3 targetPos;
    private Vector3 target2Pos;
    private float travelSpeed = 10f;
    private LayerMask ignoreRaycast;
    private float reboundDistance = 1f;
    private float maxRayDistance = 100f;
    public override void EnterState(BulletStateManager bullet, Vector3 target)
    {
        ignoreRaycast = 1 << 2;
        ignoreRaycast = ~ignoreRaycast;
        Debug.Log("Phase 3");
        bulletTransform = bullet.bulletTransform;
        targetPos = target;
        Vector3 targetDirection = targetPos - bulletTransform.position;
        bulletTransform.rotation = Quaternion.LookRotation(targetDirection);
        
    }
    public override void UpdateState(BulletStateManager bullet)
    {
        RaycastHit currentTarget;
        Vector3 targetDirection = targetPos - bulletTransform.position;
        if (Physics.Raycast(bulletTransform.position, targetDirection, out currentTarget, maxRayDistance, ignoreRaycast))
        {
            targetPos = currentTarget.point;
        }
        Debug.DrawLine(bulletTransform.position, targetPos, Color.blue);
        // Move Bullet toward target
        bulletTransform.position = Vector3.MoveTowards(bulletTransform.position, targetPos, travelSpeed * Time.deltaTime);
        // calculate rebound position
        
        Vector3 travelDirection = (targetPos - bulletTransform.position).normalized;
        if (!Physics.Raycast(bulletTransform.position, currentTarget.normal, reboundDistance))
        {
            target2Pos = bulletTransform.position + (currentTarget.normal * reboundDistance);
        }
        
    }
    public override void OnTriggerEnter(BulletStateManager bullet, Collider other)
    {
        //Debug.Log("Hit with " + other);
        if (other.tag == "Player")
        {
            //Debug.Log("ABSORB AMMO");
        }
        else if (other.tag == "Enemy")
        {
            //Debug.Log("KILL ENEMY");
        }
        else if (other.tag != "Bullet")
        {
            bullet.ChangeState(bullet.bulletPhase4, target2Pos);
        }
        // If enemy, kill enemy then go to 4
        // If player, go to 4
    }
}
