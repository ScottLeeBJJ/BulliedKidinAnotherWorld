using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCameraController : MonoBehaviour
{
    public float sensitivity = 2.0f;
    public float verticalSensitivity = 2.0f;
    public float zoomSpeed = 5.0f; // Adjust this value for zoom speed
    public float minFOV = 30.0f;  // Minimum field of view
    public float maxFOV = 60.0f;  // Maximum field of view
    public string playerTag = "Player";

    private CinemachineFreeLook freeLookCamera;
    private Transform followTarget;

    private void Start()
    {
        freeLookCamera = GetComponent<CinemachineFreeLook>();

        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            followTarget = playerObject.transform;
            freeLookCamera.Follow = followTarget;
            freeLookCamera.LookAt = followTarget;
        }
        else
        {
            Debug.LogWarning("Player object not found with tag: " + playerTag);
        }
    }

    private void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        freeLookCamera.m_XAxis.Value += mouseX * sensitivity;

        float verticalInput = Input.GetAxis("Vertical");
        freeLookCamera.m_YAxis.Value += verticalInput * verticalSensitivity;

        // Zoom using mouse scroll wheel input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        freeLookCamera.m_Lens.FieldOfView = Mathf.Clamp(freeLookCamera.m_Lens.FieldOfView - scrollInput * zoomSpeed, minFOV, maxFOV);
    }
}
