using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletStateManager : MonoBehaviour
{
    BulletBaseState currentState;

    public BulletBaseState bulletPhase1 = new BulletPhase1();
    public BulletBaseState bulletPhase2 = new BulletPhase2();
    public BulletBaseState bulletPhase3 = new BulletPhase3();
    public BulletBaseState bulletPhase4 = new BulletPhase4();

    public GameObject playerRef;
    public Transform bulletTransform;

    //Gzimos
    private Vector3 currentTarget;

    public void Initialize(Vector3 target, GameObject player)
    {
        playerRef = player;
        bulletTransform = gameObject.transform;
        currentState = bulletPhase1;
        currentTarget = target; /// GIZMOS
        currentState.EnterState(this, target);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void ChangeState(BulletBaseState newState, Vector3 target)
    {
        currentState = newState;
        currentTarget = target; /// GIZMOS
        newState.EnterState(this, target);
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(this, other);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
    }
}
