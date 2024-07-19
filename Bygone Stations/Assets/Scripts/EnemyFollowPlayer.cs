using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{

    [SerializeField] public float speed = 2f; 
    [SerializeField] public float lineOfSight = 5f; 
    [SerializeField] public float shootingRange = 3f; 
    private Transform player;
    public GameObject bullet;
    public GameObject bulletParent; 

    // maybe one enemy that does shoot and another that doesn't


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; 

    }

    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if ((distanceFromPlayer < lineOfSight) && (distanceFromPlayer > shootingRange)) {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);    
        }
        else if (distanceFromPlayer <= shootingRange) {
            Instantiate(bullet, bulletParent.transform.position, Quaternion.identity); 
            // Quaternion.identity means no rotation
        }  

    }
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lineOfSight); 
        Gizmos.DrawWireSphere(transform.position, shootingRange); 
    }
}


