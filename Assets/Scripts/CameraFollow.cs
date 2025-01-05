using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("General")]
    public Transform target; // The object to follow
    
    [Header("Target Offset")]
    public float followSmoothSpeed = 0.125f; // Smoothness for following the target
    public Vector3 defaultOffset = new Vector3(0, 15, -40); // Default position offset relative to the target

    [Header("Rotation Settings")]
    public float rotationSpeed = 30f; // Speed of rotation
    public float rotationResetSpeed = 1f; // Speed at which the camera resets to the default position
    public float maxRotationAngle = 30f; // Maximum angle the camera can rotate (in degrees)
    public bool followTargetRotation = true; // Should the camera follow the target's rotation

    private float currentYaw = 0f; // Current rotation angle (yaw) around the target
    private bool isRotating = false; // Whether the camera is currently being rotated

    void Start()
    {
        
    }

    void Update()
    {
        if (target == null)
            return;

        // Handle camera rotation input
        if (Input.GetKey(KeyCode.E)) {
            currentYaw -= rotationSpeed * Time.deltaTime;
            isRotating = true;
        } else if (Input.GetKey(KeyCode.Q)) {
            currentYaw += rotationSpeed * Time.deltaTime;
            isRotating = true;
        } else {
            isRotating = false;
        }

        // Clamp the rotation angle
        currentYaw = Mathf.Clamp(currentYaw, -maxRotationAngle, maxRotationAngle);
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate target position
        Vector3 targetPosition = target.position;

        // Follow the target with an offset
        Quaternion targetRotation = followTargetRotation ? target.rotation : Quaternion.identity;
        Vector3 targetOffset = targetRotation * defaultOffset;

        // Handle rotation
        if (isRotating) {
            // Apply user-controlled yaw rotation
            Quaternion userRotation = Quaternion.Euler(0, currentYaw, 0);
            targetOffset = userRotation * targetOffset;
        } else {
            // Smoothly reset the yaw angle
            currentYaw = Mathf.LerpAngle(currentYaw, 0, rotationResetSpeed * Time.deltaTime);
            Quaternion resetRotation = Quaternion.Euler(0, currentYaw, 0);
            targetOffset = resetRotation * targetOffset;
        }

        // Calculate desired position and rotation
        Vector3 desiredPosition = targetPosition + targetOffset;
        desiredPosition.y = transform.position.y;
        Quaternion desiredRotation = Quaternion.LookRotation(targetPosition - transform.position);

        // Smoothly move to the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSmoothSpeed);

        // Smoothly rotate towards the target
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, followSmoothSpeed);
    }
}
