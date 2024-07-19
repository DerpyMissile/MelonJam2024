using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 5;
    private float jumpPower = 5;
    private bool facingRight = true;
    private bool swinging = false;
    private float horizontal;
    private float timer = 0.0f;
    private float waitTime = 1.0f;
    private GameObject pc;
    private Rigidbody2D pc_r;
    public GameObject attackPrefab;
    public Transform groundCheck;
    public LayerMask groundLayer;
    void Awake()
    {
        pc = GameObject.FindWithTag("Player");
        pc_r = pc.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        pc_r.velocity = new Vector2(horizontal * speed, pc_r.velocity.y);

        if(!facingRight && horizontal > 0f){
            Flip();
        }else if(facingRight && horizontal < 0f){
            Flip();
        }
    }

    public void Jump(InputAction.CallbackContext context){
        if(context.performed && isGrounded()){
            pc_r.velocity = new Vector2(pc_r.velocity.x, jumpPower);
        }
        if(context.canceled && pc_r.velocity.y > 0f){
            pc_r.velocity = new Vector2(pc_r.velocity.x, pc_r.velocity.y * 0.5f);
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
    }

    public void OnFire(InputAction.CallbackContext context){
        if(context.performed && !swinging){
            StartCoroutine(AttackDrop());
            swinging = true;
        }
    }

    IEnumerator AttackDrop(){
        Vector2 newPos = pc.GetComponent<Transform>().position;
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

    IEnumerator RotateSwing(GameObject firedAttack)
    {
        float totalRotation = 45f;
        float rotationSpeed = totalRotation / waitTime;
        Vector2 currPos = pc.GetComponent<Transform>().position;

        float elapsedTime = 0.0f;
        while (elapsedTime < waitTime)
        {
            float rotationStep = rotationSpeed * Time.deltaTime;
            firedAttack.GetComponent<Transform>().RotateAround(currPos, Vector3.forward, rotationStep);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        swinging = false;
    }
}
