using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector2> MovementEvent;


    private void Update()
    {
        CheckMoveInput();
    }

    private void CheckMoveInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontal, vertical);

        MovementEvent?.Invoke(movement.normalized);
    }
}
