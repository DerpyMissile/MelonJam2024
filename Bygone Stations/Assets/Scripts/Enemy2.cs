using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
using UnityEngine;


public class Enemy2 : MonoBehaviour {
    [SerializeField] float speed; 
    public float circleRadius; 
    private Rigidbody2D EnemyRB;
    public GameObject groundCheck;
    public LayerMask groundLayer;
    public bool facingRight; 
    public bool isGrounded; 
    Vector2 direction; 


    void Start() {
        speed = 5; 
        EnemyRB = GetComponent<Rigidbody2D>(); 
        facingRight = true; 
        direction = new Vector2(90, 0);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, circleRadius, groundLayer); 
        if (!isGrounded) {
            Flip();
        }
    }

    void Flip() {
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0, 180, 0));
        speed = -speed; 
        direction = -direction; 
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.transform.position, circleRadius); 
    }
}
