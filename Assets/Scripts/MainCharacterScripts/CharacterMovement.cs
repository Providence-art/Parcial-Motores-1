using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float runSpeed = 12f;
    [SerializeField] private float jumpPower = 7f;
    [SerializeField] private float gravity = 10f;
    [SerializeField] private float lookSpeed = 5f;
    [SerializeField] private float lookXLimit = 45f;
    [SerializeField] private float defaultHeight = 2f;
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float crouchSpeed = 3f;


    private InputActionReference moveAction;
    private InputActionReference lookAction;
    private InputActionReference sprintAction;
    private InputActionReference crouchAction;
    private InputActionReference jumpAction;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;

    private void OnEnable()
    {

    moveAction = InputManagerScript.Instance.moveAction;
    lookAction = InputManagerScript.Instance.lookAction;
    sprintAction = InputManagerScript.Instance.sprintAction;
    crouchAction = InputManagerScript.Instance.crouchAction;
    jumpAction = InputManagerScript.Instance.jumpAction;
        
        
        
        
        
        moveAction.action.Enable();
        lookAction.action.Enable();
        sprintAction.action.Enable();
        crouchAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        sprintAction.action.Disable();
        crouchAction.action.Disable();
        jumpAction.action.Disable();
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CinemachineCamera vcam = FindFirstObjectByType<CinemachineCamera>();

        if (vcam != null)
        {
            vcam.Follow = playerCamera.transform;
            vcam.LookAt = playerCamera.transform;
        }


    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        bool isRunning = sprintAction.action.IsPressed();
        bool isCrouching = crouchAction.action.IsPressed();
        bool isJumping = jumpAction.action.WasPressedThisFrame();

        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * moveInput.y : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * moveInput.x : 0;
        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (isJumping && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (isCrouching && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            Vector2 lookInput = lookAction.action.ReadValue<Vector2>();

            rotationX += lookInput.y * lookSpeed * 0.1f;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, lookInput.x * lookSpeed * 2 * 0.1f, 0);
        }
    }
}