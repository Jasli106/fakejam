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
        if (Input.GetKeyDown(KeyCode.W)) {
            movement += Vector2.up;
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            movement += Vector2.down;
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            movement += Vector2.right;
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            movement += Vector2.left;
        }

        rb.velocity = speed * movement;
    }


}
