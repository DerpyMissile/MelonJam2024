using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickBurst : MonoBehaviour
{
    GameObject player;
    // public float speed = 10000f;
    // Rigidbody2D gimmickRB; 
    // Vector2 moveDirection; 

    // Start is called before the first frame update
    void Start()
    {
        // gimmickRB = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        
        // Normalized - like in math:
        // Obtains the vector's magnitude and ignores the vector's magnitude
        // moveDirection = (player.transform.position - transform.position).normalized * speed;
        // gimmickRB.velocity = new Vector2(moveDirection.x, moveDirection.y);

        // Vector2 direction = target.transform.position - transform.position;
        // transform.Translate(direction * chaseSpeed * Time.deltaTime);
        
        // Destroy(bulletRB, 2.0f); 
    }

    void Update() {
        
        // moveDirection = (player.transform.position - transform.position).normalized * speed;
        // gimmickRB.velocity = new Vector2(moveDirection.x, moveDirection.y);
        transform.position = player.transform.position; 
    }
}

// when right click
// create clone at player's position and start the animation

// when right click is released, delete clone 


// if still holding onto right click
// stay at the peak of the animation


