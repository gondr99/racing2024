using System.Collections.Generic;
using UnityEngine;

public struct RayInfo
{
    public Vector3 start;
    public Vector3 end;
}

public class CarSensor : MonoBehaviour
{
    [SerializeField] private Transform _frontSensor;
    [SerializeField] private Transform _leftSensor;
    [SerializeField] private Transform _rightSensor;

    [SerializeField] private LayerMask _whatIsObstacle;
    [SerializeField] private float _sensorDistance;
    
    private List<RayInfo> _drawList;

    public bool avoiding = false;
    private float _lastAvoidingTime;
    private void Awake()
    {
        _drawList = new List<RayInfo>();
    }

    private void Update()
    {
        CheckObstacle();

        if (avoiding && _lastAvoidingTime + 2f < Time.time)
        {
            avoiding = false;
        }
    }

    public float CheckObstacle()
    {
        RaycastHit hit;
        float steering = 0;

        bool front = RayCastTo(_frontSensor, _frontSensor.forward, out hit, _sensorDistance);
        bool left =  RayCastTo(_leftSensor, _leftSensor.forward, out hit, _sensorDistance);
        bool leftSide = RayCastTo(_leftSensor, Quaternion.AngleAxis(-30f, transform.up) *  _leftSensor.forward, out hit, _sensorDistance);
        
        bool right =  RayCastTo(_rightSensor, _rightSensor.forward, out hit, _sensorDistance);
        bool rightSide = RayCastTo(_rightSensor, Quaternion.AngleAxis(30f, transform.up) *  _rightSensor.forward, out hit, _sensorDistance);

        if (left)
            steering += 2.5f;
        if (leftSide)
            steering += 1.5f;
        if (right)
            steering -= 2.5f;
        if (rightSide)
            steering -= 1.5f;

        if (steering != 0)
        {
            avoiding = true;
            _lastAvoidingTime = Time.time;
        }

        return steering;
    }

    private bool RayCastTo(Transform sensor, Vector3 dir, out RaycastHit hit, float distance)
    {
        bool isHit = Physics.Raycast(sensor.position, dir, out hit, distance, _whatIsObstacle);
        if (isHit)
        {
            RayInfo info = new RayInfo() { start = sensor.position, end = hit.point };
            _drawList.Add(info);
        }

        return isHit;
    }

    private void OnDrawGizmos()
    {
        if (_drawList != null && _drawList.Count > 0)
        {
            foreach (RayInfo rayInfo in _drawList)
            {
                Debug.DrawLine(rayInfo.start, rayInfo.end);
            }
            
            _drawList.Clear();
        }
    }
}
