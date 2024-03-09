using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEngine : MonoBehaviour
{
    [SerializeField] private float _mortorTorque = 80f;
    [SerializeField] private float _maxSteeringAngle = 45f;
    [SerializeField] private float _maxBrakeTorque = 80f;
    [SerializeField]
    private List<Transform> _pathList;

    [SerializeField] private WheelCollider[] _wheels;
    [SerializeField] private GameObject[] _wheelMesh;
    
    
    [SerializeField]
    private int _currentNodeIndex = 0;

    public float maxSpeed = 100f;
    public float currentSpeed = 0;
    public Vector3 centerOfMass;
    private Rigidbody _rigidbody;

    private CarSensor _sensor;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = centerOfMass;
        _sensor = GetComponent<CarSensor>();
    }

    private void Start()
    {
        _pathList = Path.Instance.PathList;
    }

    private void FixedUpdate()
    {
        ApplySteering();
        DriveCar();
        AnimateWheels();
    }

    private void DriveCar()
    {
        //바퀴의 rpm에 맞추어 속도 정한다.
        currentSpeed = 2 * Mathf.PI * _wheels[0].radius * _wheels[0].rpm * 0.06f;

        if (currentSpeed < maxSpeed)
        {
            for (int i = 0; i < _wheels.Length; ++i)
            {
                _wheels[i].motorTorque = _mortorTorque;
            }    
        }
        else
        {
            for (int i = 0; i < _wheels.Length; ++i)
            {
                _wheels[i].motorTorque = 0;
            }
        }

        CheckNextWayPoint();
    }

    private void CheckNextWayPoint()
    {
        Vector3 target = _pathList[_currentNodeIndex].position;
        target.y = transform.position.y;

        float distance = Vector3.Distance(target, transform.position);
        if (distance < 4f)
        {
            _currentNodeIndex = (_currentNodeIndex + 1) % _pathList.Count;
        }

        int nextNodeIndex = GetNextIndex(_currentNodeIndex);
        float nextDistance = Vector3.Distance(_pathList[nextNodeIndex].position, transform.position);
        while (distance > nextDistance || nextDistance < 6f)
        {
            _currentNodeIndex = nextNodeIndex;
            nextNodeIndex = GetNextIndex(_currentNodeIndex);

            distance = nextDistance;
            nextDistance = Vector3.Distance(_pathList[nextNodeIndex].position, transform.position);
        }
    }

    private int GetNextIndex(int idx)
    {
        return (idx + 1) % _pathList.Count;
    }

    private void ApplySteering()
    {
        float sensorSteering = _sensor.CheckObstacle();

        
        Vector3 relativeVector = transform.InverseTransformPoint(_pathList[_currentNodeIndex].position);
        relativeVector.Normalize();

        float newSteering = relativeVector.x * _maxSteeringAngle;
        
        if (sensorSteering != 0 || _sensor.avoiding)
        {
            newSteering = sensorSteering * _maxSteeringAngle;
        }
        
        for (int i = 0; i < _wheels.Length - 2; ++i)
        {
            _wheels[i].steerAngle = newSteering;
        }
    }

    public void Braking(bool isBrake)
    {
        float brakeValue = isBrake ? _maxBrakeTorque : 0;
        
        for (int i = 2; i < _wheels.Length; ++i)
        {
            _wheels[i].brakeTorque = brakeValue;
        }
        
        Debug.Log("Brake");
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
