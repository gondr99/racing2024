using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private WheelCollider[] _wheels;
    [SerializeField] private GameObject[] _wheelMesh;

    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private float _torque = 200f;
    [SerializeField] private float _steeringMax = 4;
    public PlayerInput PlayerInput => _playerInput;

    private Vector2 _movementInput;

    private void Awake()
    {
        PlayerInput.MovementEvent += HandleMovementEvent;
    }

    private void HandleMovementEvent(Vector2 movement)
    {
        _movementInput = movement;
    }

    private void FixedUpdate()
    {
        GetAccel();
        GetHandling();
        AnimateWheels();
    }

    private void GetHandling()
    {
        for(int i = 0; i < _wheels.Length - 2; ++i)
        {
            _wheels[i].steerAngle = _movementInput.x * _steeringMax;
        }
    }

    private void GetAccel()
    {
        for (int i = 0; i < _wheels.Length; ++i)
        {
            _wheels[i].motorTorque = _movementInput.y * _torque;
        }
    }

    private void AnimateWheels()
    {
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for(int i = 0; i < _wheels.Length; ++i)
        {
            _wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);

            _wheelMesh[i].transform.SetPositionAndRotation(wheelPosition, wheelRotation);
        }
    }
}
