using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AnimationController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 2;
    [SerializeField] LayerMask groundLayer;
    Rigidbody2D rb;
    AnimationController animController;

    public Vector2 lastDirection = Vector2.down;
    public Vector2 currentPosition = Vector2.zero;

    public bool movementEnabled = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animController = GetComponent<AnimationController>();
    }

    void Update()
    {
        Vector2 movement = Vector2.zero;
        if (Input.GetKey(KeyCode.W) && movementEnabled) {
            movement += Vector2.up;
            animController.SetAnimationState("character_walk_up");
        }
        if (Input.GetKey(KeyCode.S) && movementEnabled) {
            movement += Vector2.down;
            animController.SetAnimationState("character_walk_down");
        }
        if (Input.GetKey(KeyCode.D) && movementEnabled) {
            movement += Vector2.right;
            animController.SetAnimationState("character_walk_right");
        }
        if (Input.GetKey(KeyCode.A) && movementEnabled) {
            movement += Vector2.left;
            animController.SetAnimationState("character_walk_left");
        }

        if (movement != Vector2.zero)
        {
            lastDirection = movement;
        }
        else
        {
            if (lastDirection.y > 0)
            {
                animController.SetAnimationState("character_idle_up");
            }
            else if (lastDirection.y < 0)
            {
                animController.SetAnimationState("character_idle_down");
            }
            else if (lastDirection.x > 0)
            {
                animController.SetAnimationState("character_idle_right");
            }
            else if (lastDirection.x < 0)
            {
                animController.SetAnimationState("character_idle_left");
            }
        }

        movement.Normalize();

        Vector2 newPos = rb.position + movement * speed * Time.deltaTime;

        if (IsPositionOnGround(newPos))
        {
            rb.position = newPos;
        }

        currentPosition = rb.position;
    }

    private bool IsPositionOnGround(Vector2 position)
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(position, groundLayer);
        return hitCollider != null;
    }
}
