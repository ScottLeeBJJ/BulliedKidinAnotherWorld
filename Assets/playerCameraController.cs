using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCameraController : MonoBehaviour
{
    public float sensitivity = 2.0f;
    public float verticalSensitivity = 2.0f;
    public float zoomSpeed = 5.0f;
    public float minFOV = 30.0f;
    public float maxFOV = 60.0f;
    public string playerTag = "Player";
    public float smoothing = 5.0f;
    public float verticalLimit = 80.0f;
    public KeyCode resetKey = KeyCode.R;

    private CinemachineFreeLook freeLookCamera;
    private Transform followTarget;
    private Vector3 offset;

    private bool mouseLookEnabled = true;

    private void Start()
    {
        freeLookCamera = GetComponent<CinemachineFreeLook>();

        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            followTarget = playerObject.transform;
            freeLookCamera.Follow = followTarget;
            freeLookCamera.LookAt = followTarget;

            offset = transform.position - followTarget.position;
        }
        else
        {
            Debug.LogWarning("Player object not found with tag: " + playerTag);
        }

        // Lock the cursor and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Toggle mouse look
        if (Input.GetKeyDown(KeyCode.M))
        {
            mouseLookEnabled = !mouseLookEnabled;
            Cursor.lockState = mouseLookEnabled ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !mouseLookEnabled;
        }

        if (mouseLookEnabled)
        {
            // Smoothly rotate the camera horizontally
            freeLookCamera.m_XAxis.Value += mouseX * sensitivity * Time.deltaTime * smoothing;

            // Smoothly rotate the camera vertically and apply limits
            float verticalInput = -mouseY * verticalSensitivity * Time.deltaTime * smoothing;
            float newVerticalRotation = freeLookCamera.m_YAxis.Value + verticalInput;
            freeLookCamera.m_YAxis.Value = Mathf.Clamp(newVerticalRotation, -verticalLimit, verticalLimit);
        }

        // Zoom using mouse scroll wheel input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        freeLookCamera.m_Lens.FieldOfView = Mathf.Clamp(freeLookCamera.m_Lens.FieldOfView - scrollInput * zoomSpeed, minFOV, maxFOV);

        // Reset camera position with a key press
        if (Input.GetKeyDown(resetKey))
        {
            freeLookCamera.m_XAxis.Value = 0.5f;
            freeLookCamera.m_YAxis.Value = 0.5f;
        }

        // Update camera position to maintain offset
        Vector3 desiredPosition = followTarget.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothing);
    }
}
