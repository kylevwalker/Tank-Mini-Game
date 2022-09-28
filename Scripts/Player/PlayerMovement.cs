using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // References
    private CharacterController playerController;
    private Transform playerTransform;
    private LayerMask playerLayer;
    private float maxRaycastDistance = 50.0f;
    public Camera playerCamera;
    public PlayerInput playerInput;
    public Transform bulletEmitter;
    public GameObject bulletPrefab;

    // Parameters
    public float maxSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
    public float accelTime = 0.5f;
    private float accelSpeed;

    Vector3 aimDirection = Vector3.zero;

    public bool canShoot = true;


    // Values
    private Vector3 targetSpeed = Vector3.zero;
    private float velX;
    private float velZ;


    // Gizmos Testing
    private Vector3 gizmoOrigin;
    private Vector3 gizmoGunDirection;
    private Vector3 gizmoHit;
    private Vector3 gizmoBounceDirection;
    private Vector3 gizmoHit2;

    void Awake()
    {
        //Cursor.visible = false;
        playerController = GetComponent<CharacterController>();
        playerTransform = this.transform;
        playerInput = GetComponent<PlayerInput>();
        playerLayer = 1 << 2;
        playerLayer = ~playerLayer;
        accelSpeed = maxSpeed / accelTime;
        canShoot = true;
    }

    /*
    public void OnDrawGizmos()
    {
        if(playerTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(gizmoOrigin, gizmoGunDirection);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(gizmoHit, gizmoBounceDirection);
            Gizmos.DrawSphere(gizmoHit, 0.1f);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(gizmoHit2, playerTransform.position - gizmoHit2);
            Gizmos.DrawSphere(gizmoHit2, 0.1f);
        }
        
    }
    */

    void Update()
    {
        MovePlayer(Time.deltaTime);
        
    }
    private void LateUpdate()
    {
        RotatePlayer(Time.deltaTime);
    }

    private void MovePlayer(float delta)
    {
        Vector2 moveDirection = playerInput.movementInput.normalized;
        Vector2 targetVelocity = moveDirection * maxSpeed;

        velX = Mathf.Lerp(velX, targetVelocity.x, Time.deltaTime * accelSpeed);
        velZ = Mathf.Lerp(velZ, targetVelocity.y, Time.deltaTime * accelSpeed);
        //targetSpeed = Vector2.Lerp(targetSpeed, moveDirection * maxSpeed, delta * accelSpeed);
        targetSpeed = new Vector3(velX, 0f, velZ);
          

        playerController.Move(targetSpeed * delta);
    }

    private void RotatePlayer(float delta)
    {
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
        mouseScreenPosition.z = playerCamera.nearClipPlane;
        Vector3 mouseWorldPosition = playerCamera.ScreenToWorldPoint(mouseScreenPosition);
        aimDirection = (mouseWorldPosition - playerTransform.position).normalized;
        float rotationAngle = -1 * Mathf.Atan2(aimDirection.z, aimDirection.x) * Mathf.Rad2Deg + 90f;
        Vector3 targetRotation = new Vector3(
            0,
            Mathf.LerpAngle(playerTransform.localEulerAngles.y, rotationAngle, delta * rotationSpeed),
            0);
        playerTransform.localEulerAngles = targetRotation;
    }

    public void Shoot()
    {
        if (canShoot)
        {
            
            Quaternion bulletDirection = Quaternion.Euler(0, playerTransform.localEulerAngles.y, 0);
            RaycastHit hit1;
            RaycastHit hit2;
            Vector3 gunDirection = new Vector3(aimDirection.x, 0.0f, aimDirection.z);
            if (Physics.Raycast(bulletEmitter.position, gunDirection, out hit1, maxRaycastDistance, playerLayer))
            {
                gizmoOrigin = bulletEmitter.position;
                gizmoHit = hit1.point;
                gizmoGunDirection = hit1.point - bulletEmitter.position;



                Vector3 bounceDirection = Vector3.Reflect(gunDirection, hit1.normal);
                bounceDirection = new Vector3(bounceDirection.x, 0.0f, bounceDirection.z);
            
                if (Physics.Raycast(hit1.point, bounceDirection, out hit2, maxRaycastDistance, playerLayer))
                {
                    gizmoBounceDirection = hit2.point - hit1.point;
                    gizmoHit2 = hit2.point;
                }
                CreateBullet(bulletEmitter.position, hit1.point);
            }
        }
    }
    public GameObject CreateBullet(Vector3 origin, Vector3 hit1)
    {
        GameObject bullet = Instantiate(bulletPrefab, origin, playerTransform.localRotation) as GameObject;
        bullet.GetComponent<BulletStateManager>().Initialize(hit1, this.gameObject);
        Destroy(bullet, 10f);
        return bullet;
    }
}
