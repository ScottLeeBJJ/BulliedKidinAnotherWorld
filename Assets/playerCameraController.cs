using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCameraController : MonoBehaviour
{
    public float sensitivity = 2.0f;
    public float verticalSensitivity = 2.0f; // Adjust this value for vertical rotation
    public string playerTag = "Player"; // Tag of the player object

    private CinemachineFreeLook freeLookCamera;
    private Transform followTarget;

    private void Start()
    {
        // Get the CinemachineFreeLook component of the virtual camera
        freeLookCamera = GetComponent<CinemachineFreeLook>();

        // Find the player object by tag
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            // Set the player object's transform as the follow target
            followTarget = playerObject.transform;
            freeLookCamera.Follow = followTarget;

            // Set the Look At target to the player object's transform
            freeLookCamera.LookAt = followTarget;
        }
        else
        {
            Debug.LogWarning("Player object not found with tag: " + playerTag);
        }
    }

    private void LateUpdate()
    {
        // Get mouse input for camera rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y"); // Get vertical mouse input

        // Rotate the camera horizontally based on mouse input
        freeLookCamera.m_XAxis.Value += mouseX * sensitivity;

        // Adjust the vertical rotation based on inverted sensitivity
        freeLookCamera.m_YAxis.Value += mouseY * verticalSensitivity;
    }
}
