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

    private bool isMouseLookEnabled = true;

    private const float DefaultXRotation = 1.0f;
    private const float DefaultYRotation = 1.0f;

    private void Start()
    {
        InitializeCamera();
    }

    private void InitializeCamera()
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

        LockAndHideCursor();
    }

    private void LockAndHideCursor()
    {
        Cursor.lockState = isMouseLookEnabled ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isMouseLookEnabled;
    }

    private void LateUpdate()
    {
        HandleMouseLook();
        HandleZoom();
        HandleReset();
        UpdateCameraPosition();
    }

    private void HandleMouseLook()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isMouseLookEnabled = !isMouseLookEnabled;
            LockAndHideCursor();
        }

        if (isMouseLookEnabled)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Debug log to check mouse input values
            Debug.Log("Mouse X: " + mouseX + ", Mouse Y: " + mouseY);

            // Invert the vertical rotation
            freeLookCamera.m_YAxis.Value -= mouseY * verticalSensitivity * Time.deltaTime * smoothing;

            // Invert the horizontal rotation
            freeLookCamera.m_XAxis.Value += mouseX * sensitivity * Time.deltaTime * smoothing;
        }
    }

    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        freeLookCamera.m_Lens.FieldOfView = Mathf.Clamp(
            freeLookCamera.m_Lens.FieldOfView - scrollInput * zoomSpeed, minFOV, maxFOV);
    }

    private void HandleReset()
    {
        if (Input.GetKeyDown(resetKey))
        {
            freeLookCamera.m_XAxis.Value = DefaultXRotation;
            freeLookCamera.m_YAxis.Value = DefaultYRotation;
        }
    }

    private void UpdateCameraPosition()
    {
        Vector3 desiredPosition = followTarget.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothing);
    }
}
