using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameObject player;
    public float speed = 3f;
    Rigidbody2D bulletRB; 
    Vector2 moveDirection; 

    // Start is called before the first frame update
    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        // Normalized - like in math:
        // Obtains the vector's magnitude and ignores the vector's magnitude
        moveDirection = (player.transform.position - transform.position).normalized * speed;
        Debug.Log(moveDirection.x + " " + moveDirection.y);
        bulletRB.velocity = new Vector2(moveDirection.x, moveDirection.y);

        // Vector2 direction = target.transform.position - transform.position;
        // transform.Translate(direction * chaseSpeed * Time.deltaTime);
        
        // Destroy(bulletRB, 2.0f); 
    }

    // Update is called once per frame
    void Update()
    {
        // bulletRB.velocity = new Vector2(moveDirection.x, moveDirection.y);
        // Destroy(this.gameObject, 5); 
    }
}
