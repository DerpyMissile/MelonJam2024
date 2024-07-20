using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    [SerializeField] float chaseDistance = 6f; 
    [SerializeField] float jumpDistance = 3f; 
    [SerializeField] float meleeRange = 1f;
    [SerializeField] float chaseSpeed = 0.5f; 
    [SerializeField] float jumpSpeed = 3.5f; 

    private Rigidbody2D EnemyRB;
    private float health; 
    private bool doingAction = false;
    

    GameObject player;

    private void Start() {
        player = GameObject.FindWithTag("Player"); 
        health = 1f; 
        EnemyRB = GetComponent<Rigidbody2D>(); 
    }

    private void Update() {
        if(!doingAction){
            doingAction = true;
            if(DistanceToPlayer() <= meleeRange){
                // SMACK THE PLAYER
                StartCoroutine(doAction(0));
            }
            else if (DistanceToPlayer() < chaseDistance) {
                Debug.Log("Chasing");
                Vector2 direction = player.transform.position - transform.position;
                if (direction.x > 0) {
                    direction = new Vector2(1, 0);
                }
                else {
                    direction = new Vector2(-1, 0);
                }

                transform.Translate(direction * chaseSpeed * Time.deltaTime);

                // ok so boss has 3 behaviors: do nothing, charge, jump
                int temp = (int)Mathf.Floor(Random.Range(0f, 2f));
                if(temp == 0){
                    // do nothing
                    StartCoroutine(doAction(1));
                }else if(temp == 1){
                    StartCoroutine(doAction(2));
                }else{
                    StartCoroutine(doAction(3));
                }
            }else{
                doingAction = false;
            }
        }
    }
    
    private float DistanceToPlayer(){           
        // Gives us distance between player and enemy 
        return Vector2.Distance(player.transform.position, transform.position);
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("OUCH!" + health); 
        health -= 1; 
        if (health == 0) {
            Destroy(this.gameObject, 0.2f); 
        }
        else if(this.GetComponent<Transform>().position.x < other.gameObject.GetComponent<Transform>().position.x){
            EnemyRB.velocity -= new Vector2(1, 1);
        }
        else {
            EnemyRB.velocity += new Vector2(1, 1);
        }
    }

    IEnumerator doAction(int whichAction){
        switch(whichAction){
            case 0:
                // smack player
                break;
            case 1:
                // do nothing
                break;
            case 2:
                // charge at player
                yield return new WaitForSeconds(2.0f);
                EnemyRB.velocity -= new Vector2(5,0);
                yield return new WaitForSeconds(0.5f);
                EnemyRB.velocity += new Vector2(10,0);
                break;
            case 3:
                // jump at player
                break;
        }
        yield return new WaitForSeconds(1.0f);
        doingAction = false;
    }
}
