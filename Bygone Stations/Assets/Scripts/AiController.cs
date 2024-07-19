using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// // This was originally in PlayerController
// namespace RPG.Control {

// }

// This was originally in FollowCamera.cs
// namespace RPG.Movement {
//     public class FollowCamera : MonoBehaviour {
//         [SerializeField] Transform target;

//         void LateUpdate() {
//             transform.position = target.position; 
//         }
//     }

// }

// namespace RPG.Movement() {

// }

// Will make everything within public 
// Just gotta use using namespace afterwards 
namespace RPG.Control {
    public class AIController : MonoBehaviour {
        [SerializeField] float chaseDistance = 5f; 
        [SerializeField] float jumpDistance = 3f; 

        [SerializeField] float chaseCloseDistance = 2f; 
        [SerializeField] float chaseSpeed = 0.5f; 
        [SerializeField] float jumpSpeed = 1f; 

        bool canMove = true; 
        // GameObject player = GameObject.FindWithTag("Player");
        // 5 Unity units

        GameObject player;

        private void Awake() {
            player = GameObject.FindWithTag("Player"); 
        }

        private void Update()
        {
            if (canMove) {
                if (DistanceToPlayer() < chaseDistance) {
                    // print(gameObject.name + "CHASE CHASE CHASE!!!"); 
                    Vector2 direction = player.transform.position - transform.position;
                    
                    if (direction.x > 0) {
                        direction = new Vector2(1, 0);
                    }
                    else {
                        direction = new Vector2(-1, 0);
                    }

                    // Chase regularly if outside of jump distance
                    if (DistanceToPlayer() > jumpDistance) {
                        transform.Translate(direction * chaseSpeed * Time.deltaTime);
                    }
                    // If within jump distance, 
                    else {
                        // Jump
                        if (DistanceToPlayer() > chaseCloseDistance) {
                            Invoke("Jump", 1f);   
                        }
                        // Chase regularly
                        else {
                            transform.Translate(direction * chaseSpeed * Time.deltaTime);
                        }
                        
                        // transform.Translate(direction * jumpSpeed * Time.deltaTime);

                    }
                    // else {
                    //     // delay for a bit
                    //     // Vector2 direction = player.transform.position - transform.position;
                    //     // direction = new Vector2(direction.x, 0);
                    //     // Debug.Log(direction.x); 
                         
                    // }
                }
            }
        }

        void Jump() {
            Vector2 direction = player.transform.position - transform.position;
            direction = new Vector2(direction.x, 0);
            transform.Translate(direction * jumpSpeed * Time.deltaTime);
        }

        void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.tag == "Player") {
                // Vector2 direction = player.transform.position - transform.position;
                // direction = new Vector2(-direction.x, 0); 
                // transform.Translate(direction * 0 * Time.deltaTime);
                canMove = false; 
            }
        }

        void OnCollisionExit2D(Collision2D other) {
            if (other.gameObject.tag == "Player") {
                canMove = true; 
            }
        }

        //     FindObjectOfType<PlayerController>().DisableControls(); 
         
        //     crashEffect.Play();
        //     GetComponent<AudioSource>().PlayOneShot(crashSFX);
        //     Invoke("ReloadScene", loadDelay); 
        // }

        private float DistanceToPlayer()
        {            // GameObject player = GameObject.FindWithTag("Player");
            // Gives us distance between player and enemy 
            return Vector2.Distance(player.transform.position, transform.position);
        }


        // void Chase(Vector3 targetPosition){
           
        // }
    }
}

    // public bool CanAttack(GameObject combatTarget) {
    //     if (combatTarget == null) {
    //         return false;
    //     }
    //     Health targetToTest = combatTarget.GetComponent<Health>(); 
    //     return targetToTest != null && !targetToTest.IsDead(); 
    // }
// }

// Chase. Pause at a certain distance and wait for a bit. Then jump towards player. 

// May be used in player