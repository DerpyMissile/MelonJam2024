using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    [SerializeField] float chaseDistance = 6f; 
    [SerializeField] float meleeRange = 1f;
    [SerializeField] float chaseSpeed = 0.5f; 
    [SerializeField] int rubbleVelocity = 5;

    private Rigidbody2D EnemyRB;
    private float health; 
    private bool doingAction = false;
    private bool onFloor = false;
    

    GameObject player;
    public GameObject attackPrefab;
    public GameObject stones;
    public GameObject exit;
    

    private void Start() {
        player = GameObject.FindWithTag("Player"); 
        health = 10f; 
        EnemyRB = GetComponent<Rigidbody2D>(); 
    }

    private void Update() {
        if(!doingAction){
            doingAction = true;
            if(DistanceToPlayer() <= meleeRange){
                // SMACK THE PLAYER
                Debug.Log("Smacng player");
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
                int temp = (int)Mathf.Floor(Random.Range(0f, 3f));
                Debug.Log("temp's: " + temp);
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

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.layer == 3){
            onFloor = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision){
        onFloor = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Bullet"){
        }else{
            Debug.Log("OUCH!" + health); 
            health -= 1; 
            if (health == 0) {
                Instantiate(exit, transform.position, Quaternion.identity);
                Destroy(this.gameObject, 0.2f); 
            }
            else if(this.GetComponent<Transform>().position.x < other.gameObject.GetComponent<Transform>().position.x){
                EnemyRB.velocity -= new Vector2(1, 1);
            }
            else {
                EnemyRB.velocity += new Vector2(1, 1);
            }
        }
    }

    IEnumerator doAction(int whichAction){
        switch(whichAction){
            case 0:
                // smack player
                yield return new WaitForSeconds(0.5f);
                Vector2 newPos = new Vector2(0, 0);
                if(player.transform.position.x < transform.position.x){
                    newPos = new Vector2(transform.position.x - (meleeRange/2), transform.position.y+1);
                }else{
                    newPos = new Vector2(transform.position.x + (meleeRange/2), transform.position.y+1);
                }
                GameObject attackEnem = Instantiate(attackPrefab, newPos, Quaternion.identity); 
                yield return new WaitForSeconds(1.0f);
                Destroy(attackEnem);
                break;
            case 1:
                // do nothing
                break;
            case 2:
                // charge at player
                yield return new WaitForSeconds(2.0f);
                if(player.transform.position.x < transform.position.x){
                    EnemyRB.velocity += new Vector2(5,0);
                    yield return new WaitForSeconds(0.5f);
                    EnemyRB.velocity -= new Vector2(10,0);
                }else{
                    EnemyRB.velocity -= new Vector2(5,0);
                    yield return new WaitForSeconds(0.5f);
                    EnemyRB.velocity += new Vector2(10,0);
                }
                
                break;
            case 3:
                // jump at player
                yield return new WaitForSeconds(1.0f);
                StartCoroutine(RotateSwing(EnemyRB.gameObject));
                break;
        }
        yield return new WaitForSeconds(1.0f);
        doingAction = false;
    }

    IEnumerator RotateSwing(GameObject firedAttack){
        float waitTime = 0.5f;
        float totalRotation = 90f;
        float rotationSpeed = totalRotation / waitTime;
        Vector2 currPos = player.GetComponent<Transform>().position;
        if(currPos.y > 4){
            currPos = new Vector2(currPos.x, 4);
        }else{

        }
        

        float elapsedTime = 0.0f;
        if(player.transform.position.x < transform.position.x){
            while (elapsedTime < waitTime){
                float rotationStep = rotationSpeed * Time.deltaTime;
                firedAttack.GetComponent<Transform>().RotateAround(currPos, Vector3.forward, rotationStep);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }else{
            while (elapsedTime < waitTime){
                float rotationStep = rotationSpeed * Time.deltaTime;
                firedAttack.GetComponent<Transform>().RotateAround(currPos, -Vector3.forward, rotationStep);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        yield return new WaitUntil(() => onFloor);
        for(int i = 0; i < Random.Range(0, 7); ++i){
            float newXPos = this.transform.position.x + Random.Range(-1f, 1f);
            GameObject rubble = Instantiate(stones, new Vector3(newXPos, this.GetComponent<Renderer>().bounds.min.y, this.transform.position.z), Quaternion.identity);
            rubble.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(0.0f, 1.0f) * rubbleVelocity, Random.Range(0.0f, 1.0f) * rubbleVelocity);
            yield return new WaitForSeconds(1.0f);
            Destroy(rubble);
        }
    }
}
