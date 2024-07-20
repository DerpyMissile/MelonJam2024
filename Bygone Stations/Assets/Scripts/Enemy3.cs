using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{

    [SerializeField] public float speed = 2f; 
    [SerializeField] public float lineOfSight = 5f; 
    [SerializeField] public float shootingRange = 3f; 
    private Transform player;
    public GameObject bullet;
    public GameObject bulletParent; 

    public float fireRate = 1f; 
    private float nextFireTime; 

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
        else if ((distanceFromPlayer <= shootingRange) && (nextFireTime < Time.time)) {
            GameObject firedBullet = Instantiate(bullet, bulletParent.transform.position, Quaternion.identity); 
            // Quaternion.identity means no rotation
            StartCoroutine(destroyBullet(firedBullet));
            nextFireTime = Time.time + fireRate;
        }  
    }

    // Coroutines happen at the same time as other functions, like Update() 
    IEnumerator destroyBullet(GameObject gameObject){
        // The return for coroutines 
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lineOfSight); 
        Gizmos.DrawWireSphere(transform.position, shootingRange); 
    }
}


