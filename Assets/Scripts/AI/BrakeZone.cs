using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeZone : MonoBehaviour
{
    [SerializeField] private BoxCollider _collider;

    private List<AIEngine> _targetList = new List<AIEngine>();
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter");
        if (other.attachedRigidbody.TryGetComponent<AIEngine>(out AIEngine ai))
        {
            _targetList.Add(ai);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("enter");
        if (other.attachedRigidbody.TryGetComponent<AIEngine>(out AIEngine ai))
        {
            _targetList.Remove(ai);
        }
    }

    private void Start()
    {
        StartCoroutine(BrakeTargets());
    }

    private IEnumerator BrakeTargets()
    {
        WaitForSeconds sec = new WaitForSeconds(0.05f);
        while (true)
        {
            Debug.Log("co");
            yield return sec;
            
            foreach (AIEngine ai in _targetList)
            {
                ai.Braking(true);
            }    
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_collider.center, _collider.size);
    }
}
