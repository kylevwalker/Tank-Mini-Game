using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhase1 : BulletBaseState
{
    private Transform bulletTransform;
    private Vector3 targetPos;
    private Vector3 target2Pos;
    private float travelSpeed = 10f;
    private LayerMask ignoreRaycast;
    private float maxRayDistance = 100f;
    public override void EnterState(BulletStateManager bullet, Vector3 target)
    {
        bulletTransform = bullet.bulletTransform;
        targetPos = target;
        ignoreRaycast = 1 << 2;
        ignoreRaycast = ~ignoreRaycast;      
    }
    public override void UpdateState(BulletStateManager bullet)
    {
        // Travel toward aim position
        RaycastHit currentTarget;
        Vector3 targetDirection = targetPos - bulletTransform.position;
        if (Physics.Raycast(bulletTransform.position, targetDirection, out currentTarget, maxRayDistance, ignoreRaycast))
        {
            targetPos = currentTarget.point;
        }
        bulletTransform.position = Vector3.MoveTowards(bulletTransform.position, targetPos, travelSpeed * Time.deltaTime);
        Debug.DrawLine(bulletTransform.position, targetPos, Color.green);
        // Calculate next target based on Vector direction reflected raycast
        Vector3 travelDirection = targetPos - bulletTransform.position;
        RaycastHit hit1;
        RaycastHit hit2;
        if (Physics.Raycast(bulletTransform.position, travelDirection, out hit1, maxRayDistance, ignoreRaycast))
        {
            Vector3 bounceDirection = Vector3.Reflect(travelDirection, hit1.normal);
            if(Physics.Raycast(hit1.point, bounceDirection, out hit2, maxRayDistance, ignoreRaycast))
            {
                target2Pos = hit2.point;
            }
        }

    }
    public override void OnTriggerEnter(BulletStateManager bullet, Collider other)
    {
        //Debug.Log("Hit with " + other);
        if(other.tag == "Player")
        {
            //Debug.Log("SELF HIT");
        }
        else if(other.tag == "Enemy")
        {
            //Debug.Log("HIT ENEMY");
        }
        else if (other.tag != "Bullet")
        {
            bullet.ChangeState(bullet.bulletPhase2, target2Pos);
        }
        // If enemy, change to state 4
        // If wall, change to state 2
    }

}

