using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private int speed = 5;
    private GameObject pc;
    void Awake()
    {
        pc = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMove(InputValue input){
        Rigidbody2D pc_r = pc.GetComponent<Rigidbody2D>();
        pc_r.velocity = input.Get<Vector2>() * speed;
    }
}
