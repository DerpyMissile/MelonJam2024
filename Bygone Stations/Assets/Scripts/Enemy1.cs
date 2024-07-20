using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// Will make everything within public 
// Just gotta use using namespace afterwards 
namespace RPG.Control {
    public class Enemy1 : MonoBehaviour {
        [SerializeField] float chaseDistance = 6f; 
        [SerializeField] float jumpDistance = 3f; 
        [SerializeField] float chaseSpeed = 0.5f; 
        [SerializeField] float jumpSpeed = 3.5f; 

        private Rigidbody2D EnemyRB;
        private float health; 
        

        GameObject player;

        private void Start() {
            player = GameObject.FindWithTag("Player"); 
            health = 1f; 
            EnemyRB = GetComponent<Rigidbody2D>(); 
        }

        private void Update() {
            if (DistanceToPlayer() < chaseDistance) {
                Vector2 direction = player.transform.position - transform.position;
                if (direction.x > 0) {
                    direction = new Vector2(1, 0);
                }
                else {
                    direction = new Vector2(-1, 0);
                }

                if (DistanceToPlayer() > jumpDistance) {
                    transform.Translate(direction * chaseSpeed * Time.deltaTime);
                }
                else {
                    transform.Translate(direction * jumpSpeed * Time.deltaTime);
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
                EnemyRB.velocity -= new Vector2(5, 1);
            }
            else {
                EnemyRB.velocity += new Vector2(5, 1);
            }
        }
    }
}


// Chase. Pause at a certain distance and wait for a bit. Then jump towards player. 
