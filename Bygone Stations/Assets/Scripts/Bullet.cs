using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    GameObject target;
    public float speed = 3f;
    Rigidbody2D bulletRB; 

    Vector2 moveDirection; 

    // Start is called before the first frame update
    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        moveDirection = (target.transform.position - transform.position).normalized * speed;
        bulletRB.velocity = new Vector2(moveDirection.x, moveDirection.y);

        // Vector2 direction = target.transform.position - transform.position;
        // transform.Translate(direction * chaseSpeed * Time.deltaTime);
        
        Destroy(this, 2); 
    }

    // Update is called once per frame
    void Update()
    {
        // bulletRB.velocity = new Vector2(moveDirection.x, moveDirection.y);
        // Destroy(this.gameObject, 5); 
    }
}
