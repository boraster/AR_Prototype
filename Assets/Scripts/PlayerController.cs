using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

// [RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    public TextMeshProUGUI position;
    public GameObject ArCameraGO; 
    
    [SerializeField]
    private float movementSpeed = 7.0f;

    private Transform rotator;
    private StringBuilder positionText;
    [SerializeField] 
    private float smoothing = 0.2f;
    
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        positionText = new StringBuilder();
        InputSystem.EnableDevice(Accelerometer.current);
        InputSystem.EnableDevice(AttitudeSensor.current);

        rotator = new GameObject("Rotator").transform;
        rotator.SetPositionAndRotation(transform.position, transform.rotation);
    }
    
    private void Update()
    {
        Move();
        LookAround();
        position.text = $"X:{transform.position.x:#0.00} Y:{transform.position.y:#0.00} Z:{-transform.position.z:#0.00}";
    }
    private void Move()
    {
        Vector3 acceleration = Accelerometer.current.acceleration.ReadValue();

        Vector3 moveDirection = new(acceleration.x * movementSpeed * Time.deltaTime, 0, -acceleration.z * movementSpeed * Time.deltaTime);
        Vector3 transformedDirection = transform.TransformDirection(moveDirection);
        transform.Translate(transformedDirection);
        ArCameraGO.transform.Translate(transformedDirection);
        // characterController.Move(transformedDirection);
    }
    private void LookAround()
    {
        Quaternion attitude = AttitudeSensor.current.attitude.ReadValue();

        rotator.rotation = attitude;
        rotator.Rotate(0f, 0f, 180f, Space.Self);
        rotator.Rotate(90f, 180f, 0f, Space.World);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotator.rotation, smoothing);
    }
}
