using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 2;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 movement = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) {
            movement += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S)) {
            movement += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D)) {
            movement += Vector2.right;
        }
        if (Input.GetKey(KeyCode.A)) {
            movement += Vector2.left;
        }

        movement.Normalize();
        rb.velocity = speed * movement;
    }


}
