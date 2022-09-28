using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhase2 : BulletBaseState
{
    private Transform bulletTransform;
    private Vector3 targetPos;
    private Vector3 target2Pos;
    private float travelSpeed = 10f;
    private LayerMask ignoreRaycast;
    private float maxRayDistance = 100f;
    public override void EnterState(BulletStateManager bullet, Vector3 target)
    {
        Debug.Log("Phase 2");
        ignoreRaycast = 1 << 2;
        ignoreRaycast = ~ignoreRaycast;
        bulletTransform = bullet.bulletTransform;
        targetPos = target;
        Vector3 targetDirection = targetPos - bulletTransform.position;
        bulletTransform.rotation = Quaternion.LookRotation(targetDirection);
    }
    public override void UpdateState(BulletStateManager bullet)
    {
        //Realtime calculating target
        RaycastHit currentTarget;
        Vector3 targetDirection = targetPos - bulletTransform.position;
        if (Physics.Raycast(bulletTransform.position, targetDirection, out currentTarget, maxRayDistance, ignoreRaycast))
        {
            targetPos = currentTarget.point;
        }
        Debug.DrawLine(bulletTransform.position, targetPos, Color.red);
        bulletTransform.position = Vector3.MoveTowards(bulletTransform.position, targetPos, travelSpeed * Time.deltaTime);

        RaycastHit hit;
        Vector3 travelDirection = bullet.playerRef.transform.position - bulletTransform.position;
        if (Physics.Raycast(bulletTransform.position, travelDirection, out hit, maxRayDistance, ignoreRaycast))
        {
            
            float bounceAngle = Vector3.Angle(travelDirection, hit.point);
           
            if(bounceAngle < 90)
            {
                target2Pos = hit.point;
            }
            else
            {
                RaycastHit hit2;
                Vector3 bounceDirection = -travelDirection;
                if(Physics.Raycast(hit.point, bounceDirection, out hit2, maxRayDistance, ignoreRaycast))
                {
                    target2Pos = hit2.point;
                }
            }
        }
    }
    public override void OnTriggerEnter(BulletStateManager bullet, Collider other)
    {
        //Debug.Log("Hit with " + other);
        if (other.tag == "Player")
        {
            //Debug.Log("KILL SELF");
        }
        else if (other.tag == "Enemy")
        {
            //Debug.Log("KILL ENEMY");
        }
        else if (other.tag != "Bullet")
        {
            bullet.ChangeState(bullet.bulletPhase3, target2Pos);
        }
        // If enemy or player, change to state 4
        // If wall, change to state 3
    }
}

