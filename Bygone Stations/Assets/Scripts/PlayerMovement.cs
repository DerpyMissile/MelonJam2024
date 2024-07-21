using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; 


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float jumpPower = 7;
    private bool facingRight = true;
    private bool swinging = false;
    private float horizontal;
    private bool notMoving = false;
    private bool sprinting = false;
    private bool flowing = false;
    private float timer = 0.0f;
    private float flowTimer = 0.0f;
    private float flowOver = 1.0f;
    private float waitTime = 1.0f;
    private float sprintTime = 0.0f;
    private float needToSprintTime = 2.0f;
    private GameObject pc;
    private Rigidbody2D pc_r;
    public GameObject attackPrefab;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public StatUI statUI;

    public GameObject gimmickParent; 

    public GameObject gimmickBurst; 
    GameObject newGimmickBurst; 
    // public Animator myAnim; 
    void Awake()
    {
        pc = GameObject.FindWithTag("Player");
        pc_r = pc.GetComponent<Rigidbody2D>();
        gimmickBurst.GetComponent<Renderer>().enabled = false; 
        // myAnim = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        // myAnim.Play("PlayerRunning");
        timer += Time.deltaTime;
        pc_r.velocity = new Vector2(horizontal * speed, pc_r.velocity.y);

        if(!facingRight && horizontal > 0f){
            Flip();
        }else if(facingRight && horizontal < 0f){
            Flip();
        }

        if(notMoving){
            flowTimer += Time.deltaTime;
            if(flowTimer > flowOver){
                flowTimer = 0;
                PlayerStats.DecreaseFlow(5);
                statUI.ChangeFlow(PlayerStats.GetFlow());
            }
        }

        if(sprinting){
            sprintTime += Time.deltaTime;
            if(sprintTime > needToSprintTime){
                sprintTime = 0.0f;
                PlayerStats.DecreaseFlow(-1);
                statUI.ChangeFlow(PlayerStats.GetFlow());
            }
        }

        if(flowing){
            flowOver = 1.0f;
            flowTimer += Time.deltaTime;
            if(flowTimer > flowOver){
                flowTimer = 0;
                PlayerStats.DecreaseFlow(3);
                statUI.ChangeFlow(PlayerStats.GetFlow());
            }
        }else{
            flowOver = 3.0f;
        }
    }

    public void Jump(InputAction.CallbackContext context){
        // myAnim.Play("PlayerJump");
        if(context.performed && isGrounded()){
            pc_r.velocity = new Vector2(pc_r.velocity.x, jumpPower);
        }
        if(context.canceled && pc_r.velocity.y > 0f){
            pc_r.velocity = new Vector2(pc_r.velocity.x, pc_r.velocity.y * 0.5f);
        }

        if(context.performed){
            flowTimer = 0;
            notMoving = false;
        }
        if(context.canceled){
            notMoving = true;
        }
    }

    private bool isGrounded(){
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip(){
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void OnMove(InputAction.CallbackContext context){
        horizontal = context.ReadValue<Vector2>().x;
        if(context.performed){
            flowTimer = 0;
            notMoving = false;
        }
        if(context.canceled){
            notMoving = true;
        }
    }

    public void OnFire(InputAction.CallbackContext context){
        if(context.performed && !swinging){
            StartCoroutine(AttackDrop());
            swinging = true;
        }
    }

    public void OnShift(InputAction.CallbackContext context){
        if(context.performed){
            sprinting = true;
            speed = 7;
            jumpPower = 10;
        }

        if(context.canceled){
            sprinting = false;
            speed = 5;
            jumpPower = 7;
        }
    }

    public void OnRclick(InputAction.CallbackContext context){
        
        if(context.performed){
            gimmickBurst.GetComponent<Renderer>().enabled = true; 
            flowing = true;
            Time.timeScale = 0.5f;
            speed *= 2;
            jumpPower *= 2;
            waitTime /= 2;

            newGimmickBurst = Instantiate(gimmickBurst, gimmickParent.transform.position, Quaternion.identity); 
        }

        if(context.canceled){
            gimmickBurst.GetComponent<Renderer>().enabled = false; 
            flowing = false;
            Time.timeScale = 1.0f;
            speed *= 0.5f;
            jumpPower *= 0.5f;
            waitTime *= 2;
            Destroy(newGimmickBurst); 
        }
    }


    
/*else if ((distanceFromPlayer <= shootingRange) && (nextFireTime < Time.time)) {
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

*/ 

    IEnumerator AttackDrop(){
        Vector2 newPos = pc.GetComponent<Transform>().position;
        // myAnim.Play("PlayerAttack"); 
        if(facingRight){
            newPos += Vector2.right;
        }else{
            newPos += Vector2.left;
        }
        // switch(directionFacing){
        //     case 0:
        //         whereSwing = Vector2.right;
        //         newPos += Vector2.right;
        //         break;
        //     case 1:
        //         whereSwing = Vector2.left;
        //         newPos += Vector2.left;
        //         break;
        //     case 2:
        //         whereSwing = Vector2.up;
        //         newPos += Vector2.up;
        //         break;
        //     case 3:
        //         whereSwing = Vector2.down;
        //         newPos += Vector2.down;
        //         break;
        // }
        GameObject firedAttack = Instantiate(attackPrefab, newPos, Quaternion.identity);
        StartCoroutine(RotateSwing(firedAttack));
        yield return new WaitForSeconds(1.0f);
        Destroy(firedAttack);
    }

    IEnumerator RotateSwing(GameObject firedAttack){
        float totalRotation = 45f;
        float rotationSpeed = totalRotation / waitTime;
        Vector2 currPos = pc.GetComponent<Transform>().position;

        float elapsedTime = 0.0f;
        while (elapsedTime < waitTime){
            float rotationStep = rotationSpeed * Time.deltaTime;
            firedAttack.GetComponent<Transform>().RotateAround(currPos, Vector3.forward, rotationStep);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        swinging = false;
    }

    void OnCollisionEnter2D(Collision2D collision){
        Debug.Log("Hit " + collision.gameObject.name);

        // Ht enemy
        if(collision.gameObject.layer == 6){
            if (collision.gameObject.tag == "Floor Enemy") {
                Debug.Log("Hit Enemy!!!111!!!");
                if(pc.GetComponent<Transform>().position.x < collision.gameObject.GetComponent<Transform>().position.x){
                    pc_r.velocity -= new Vector2(5, 1);
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(5, 1);
                }else{
                    pc_r.velocity += new Vector2(5, 1);
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity -= new Vector2(5, 1);
                }
            }

            PlayerStats.DecreaseHp(10);
            PlayerStats.DecreaseFlow(1);
            statUI.ChangeHP(PlayerStats.GetHp());
            if (PlayerStats.GetHp() == 0) {
                SceneManager.LoadScene(1); 
            }
        }

        // Ht nteractable
        // if(collision.gameObject.layer == 7){
        //     PlayerStats.touchingInteractable = true;
        //     PlayerStats.touchingWhat = collision.gameObject;
        // }
    }

    void OnCollisionExit2D(Collision2D collision){
        PlayerStats.touchingRoom = false;
        PlayerStats.touchingBench = false; 
        PlayerStats.touchingInteractable = false;
        // PlayerStats.touchingWhat = null;
        PlayerStats.touchingExit = false;
    }

    void OnTriggerEnter2D(Collider2D collision){
        // Hit interactable layer
        if(collision.gameObject.layer == 7){
            // Hit door
            if (collision.gameObject.tag == "Door") {
                Debug.Log("DOOR DOOR DOOR"); 
                PlayerStats.touchingRoom = true;
                PlayerStats.touchingWhat = collision.gameObject;
            }
            // Hit Bench 
            else if (collision.gameObject.tag == "Bench") {
                Debug.Log("BENCH BENCH BENCH"); 
                PlayerStats.touchingBench = true;
                PlayerStats.touchingWhat = collision.gameObject;
            }
            // Code interactable items 
            else{
                PlayerStats.touchingInteractable = true;
                PlayerStats.touchingWhat = collision.gameObject;
            }
        }
        // Bullet hit player
        if (collision.gameObject.tag == "Bullet") {
            PlayerStats.DecreaseHp(5);
            PlayerStats.DecreaseFlow(1);
            statUI.ChangeHP(PlayerStats.GetHp());
            if (PlayerStats.GetHp() == 0) {
                SceneManager.LoadScene(1); 
            }
        }

        if(collision.gameObject.tag == "Exit"){
            PlayerStats.touchingExit = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision){
        PlayerStats.touchingRoom = false;
        PlayerStats.touchingBench = false; 
        PlayerStats.touchingInteractable = false;
        PlayerStats.touchingWhat = null;
        PlayerStats.touchingExit = false;
    }
}

