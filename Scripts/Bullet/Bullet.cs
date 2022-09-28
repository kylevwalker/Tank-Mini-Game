using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform bulletTransform;
    public GameObject playerRef;
    public float bulletLifetime = 5f;

    private enum Phase {phase0, phase1, phase2, phase3}
    Phase currentPhase;

    private Vector3 bulletOrigin;
    private Vector3 target1;
    private Vector3 target2;
    private Vector3 target3;

    private float travelSpeed = 7f;

    private void Awake()
    {
        currentPhase = Phase.phase0;
        bulletTransform = this.transform;
        StartCoroutine("BulletLifetime");
    }
    private void Update()
    {
        BulletMovement(Time.deltaTime);
    }

    public void Initialize(Vector3 origin, Vector3 hit1, Vector3 hit2, GameObject player)
    {
        bulletOrigin = origin;
        target1 = hit1;
        target2 = hit2;
        playerRef = player;
        currentPhase = Phase.phase1;
    }
    private void BulletMovement(float delta)
    {
        if (currentPhase == Phase.phase1)
        {
            bulletTransform.position = Vector3.MoveTowards(bulletTransform.position, target1, travelSpeed * delta);
        }
        else if (currentPhase == Phase.phase2)
        {
            bulletTransform.position = Vector3.MoveTowards(bulletTransform.position, target2, travelSpeed * delta);
        }
        else if (currentPhase == Phase.phase3)
        {
            bulletTransform.position = Vector3.MoveTowards(bulletTransform.position, target3, travelSpeed * delta);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        print("Trigger with " + other);
        Vector3 hit = bulletTransform.position;
       
        // Detect self collision
        if (other == playerRef.GetComponent<CapsuleCollider>())
        {
            print("Hit self");
        }


        if (currentPhase == Phase.phase1)
        {
            Phase1Process(hit);
        }
        else if (currentPhase == Phase.phase2)
        {
            Phase2Process(hit);
        }
        else if (currentPhase == Phase.phase3)
        { 
            Phase3Process(hit);
        }
    }

    private void Phase1Process(Vector3 hit)
    {
        currentPhase = Phase.phase2;
    }
    private void Phase2Process(Vector3 hit)
    {
        currentPhase = Phase.phase3;
        target3 = playerRef.transform.position;
        
    }
    private void Phase3Process(Vector3 hit)
    {

    }


    private IEnumerator BulletLifetime()
    {
        yield return new WaitForSeconds(bulletLifetime);
        DespawnBullet();
    }

    private void DespawnBullet()
    {
        Destroy(this.gameObject);
    }

    
}
