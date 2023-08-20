using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 5.0f;
    private float jumpHeight = 5.0f;
    private float gravityValue = -9.81f;
    private Transform cameraTransform;

    private const float DefaultJumpMultiplier = -2.0f;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        cameraTransform = Camera.main.transform; // Cache camera transform
    }

    void Update()
    {
        HandleGroundedAndVelocity();
        HandlePlayerMovement();
        HandleJump();
        ApplyGravityAndMove();

        // Debug logs for jump input and grounded status
        Debug.Log("Grounded: " + groundedPlayer);
        Debug.Log("Jump Input: " + Input.GetButtonDown("Jump"));
    }

    private void HandleGroundedAndVelocity()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
    }

    private void HandlePlayerMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate the movement direction based on camera orientation
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        Vector3 moveDirection = (forward * moveVertical + right * moveHorizontal).normalized;

        // Move the player using the calculated movement direction
        controller.Move(moveDirection * Time.deltaTime * playerSpeed);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * DefaultJumpMultiplier * gravityValue);
        }
    }

    private void ApplyGravityAndMove()
    {
        // Apply gravity and move player
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
