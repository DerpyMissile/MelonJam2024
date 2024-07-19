using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private int speed = 5;
    private int directionFacing = 0;
    private float timer = 0.0f;
    private float waitTime = 1.0f;
    private GameObject pc;
    public GameObject attackPrefab;
    void Awake()
    {
        pc = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    // direction facing: 0=right, 1=left, 2=up, 3=down
    void OnMove(InputValue input){
        Rigidbody2D pc_r = pc.GetComponent<Rigidbody2D>();
        pc_r.velocity = input.Get<Vector2>() * speed;

        if(input.Get<Vector2>() == Vector2.down){
            directionFacing = 3;
        }else if(input.Get<Vector2>() == Vector2.up){
            directionFacing = 2;
        }else if(input.Get<Vector2>() == Vector2.left){
            directionFacing = 1;
        }else if(input.Get<Vector2>() == Vector2.right){
            directionFacing = 0;
        }
    }

    void OnFire(){
        StartCoroutine(AttackDrop());
    }

    IEnumerator AttackDrop(){
        Vector2 whereSwing = new Vector2(0, 0);
        Vector2 newPos = pc.GetComponent<Transform>().position;
        switch(directionFacing){
            case 0:
                whereSwing = Vector2.right;
                newPos += Vector2.right;
                break;
            case 1:
                whereSwing = Vector2.left;
                newPos += Vector2.left;
                break;
            case 2:
                whereSwing = Vector2.up;
                newPos += Vector2.up;
                break;
            case 3:
                whereSwing = Vector2.down;
                newPos += Vector2.down;
                break;
        }
        GameObject firedAttack = Instantiate(attackPrefab, newPos, Quaternion.identity);
        StartCoroutine(RotateSwing(firedAttack, whereSwing));
        yield return new WaitForSeconds(1.0f);
        Destroy(firedAttack);
    }

    IEnumerator RotateSwing(GameObject firedAttack, Vector2 whereSwing)
    {
        float totalRotation = 45f; // Total rotation angle in degrees
        float rotationSpeed = totalRotation / waitTime; // Angular speed in degrees per second
        Vector2 currPos = pc.GetComponent<Transform>().position;

        float elapsedTime = 0.0f;
        while (elapsedTime < waitTime)
        {
            float rotationStep = rotationSpeed * Time.deltaTime; // Calculate the rotation step for this frame
            firedAttack.GetComponent<Transform>().RotateAround(currPos, Vector3.forward, rotationStep);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }
    }
}
