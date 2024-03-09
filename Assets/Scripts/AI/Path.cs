using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public static Path Instance;
    
    public Color lineColor;

    [SerializeField]
    private List<Transform> _nodes = new List<Transform>();

    public List<Transform> PathList => _nodes;

    private void Awake()
    {
        Instance = this;
    }

    private void OnValidate()
    {
        if (_nodes.Count == 0)
        {
            GetComponentsInChildren<Transform>(_nodes);
            _nodes.RemoveAt(0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        Transform before = _nodes[0];
        for (int i = 1; i < _nodes.Count; ++i)
        {
            Gizmos.DrawWireSphere(before.position, 1f);
            Gizmos.DrawLine(before.position, _nodes[i].position);
            before = _nodes[i];
        }
        Gizmos.DrawWireSphere(before.position, 1f);
        Gizmos.DrawLine(before.position, _nodes[0].position);
        Gizmos.color = Color.white;
    }
}
