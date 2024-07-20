using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public Vector2 position = Vector2.zero;
    [HideInInspector] public float speed = 2;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W)) {
            position.y += speed;
        }
        if(Input.GetKeyDown(KeyCode.S)) {
            position.y -= speed;
        }
        if(Input.GetKeyDown(KeyCode.D)) {
            position.x += speed;
        }
        if(Input.GetKeyDown(KeyCode.A)) {
            position.x -= speed;
        }
        transform.position = position;
    }


}
