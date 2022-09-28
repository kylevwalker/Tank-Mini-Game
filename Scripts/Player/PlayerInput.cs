using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;
    private PlayerMovement playerScript;
    public Vector2 movementInput { get; private set; } = Vector2.zero;
    
    public float shootCooldownTimer = 1.0f;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerScript = GetComponent<PlayerMovement>();
        /* toggle buttons
        playerInputActions.Player.AimIn.performed += context => SetAimTrue();
        playerInputActions.Player.AimIn.canceled += context => SetAimFalse();
        */
        playerInputActions.Player.Shoot.performed += context => SetFire();
    }
   
    private void OnEnable()
    {

        playerInputActions.Player.Enable(); // toggles ranged controls

        playerInputActions.Player.Movement.performed += SetMovement;
        playerInputActions.Player.Movement.canceled += SetMovement;


    }
    private void OnDisable()
    {
        playerInputActions.Player.Movement.performed -= SetMovement;
        playerInputActions.Player.Movement.canceled -= SetMovement;


        playerInputActions.Player.Disable();
    }

    private void SetMovement(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void SetFire()
    {
        playerScript.Shoot();
        if (playerScript.canShoot)
        {
            StartCoroutine("ShootCooldown");
        }
    }

    private IEnumerator ShootCooldown()
    {
        playerScript.canShoot = false;
        yield return new WaitForSeconds(shootCooldownTimer);
        playerScript.canShoot = true;
    }


}
