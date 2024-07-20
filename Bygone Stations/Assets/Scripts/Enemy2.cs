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

    private float health; 


    void Start() { 
        EnemyRB = GetComponent<Rigidbody2D>(); 
        facingRight = true; 
        direction = new Vector2(90, 0);
        health = 2; 
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

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("OUCH!" + health); 
        health -= 1; 
        if (health == 0) {
            Destroy(this.gameObject, 0.2f); 
        }
        else if(this.GetComponent<Transform>().position.x < other.gameObject.GetComponent<Transform>().position.x){
            // this.velocity -= new Vector2(5, 1);
            // other.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(5, 1);
            // If enemy is to the left of player 
            // Knockback to the left
            EnemyRB.velocity -= new Vector2(5, 1);
        }
        else {
            // this.velocity += new Vector2(5, 1);
            // other.gameObject.GetComponent<Rigidbody2D>().velocity -= new Vector2(5, 1);
            EnemyRB.velocity += new Vector2(5, 1);
        }
    }
}
