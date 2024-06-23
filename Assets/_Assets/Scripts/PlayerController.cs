using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float maxSwipeTime;
    public float minSwipeDistance;
    public Animator animator;

    private Rigidbody2D playerRB;
    private Transform transform;

    private float swipeStartTime;
    private float swipeEndTime;

    private Vector2 startSwipePosition;
    private Vector2 endSwipePosition;

    private float swipeTime;
    private float swipeLength;
    private bool isRight = false;
    private bool isLeft = false;

    private float[] playerPosition = { -12.54f, -6.34f, -0.14f, 6.06f, 12.26f};
    private int playerPositionIndex = 1;


    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRight && playerPositionIndex < 4)
        {
            isLeft = false;
            playerRB.velocity = new Vector2(1 * speed * Time.deltaTime, playerRB.velocity.y);
            swipeRightPosition();
        } else if (isLeft && playerPositionIndex > 0)
        {
            isRight = false;
            playerRB.velocity = new Vector2(-1 * speed * Time.deltaTime, playerRB.velocity.y);
            swipeLeftPosition();
        }
        
        swipeCheck();
    }

    private void swipeRightPosition()
    {
        if (transform.position.x >= playerPosition[playerPositionIndex + 1])
        {
            Debug.Log("Index before movement is --> " + playerPositionIndex);
            stopMovement();
            if(playerPositionIndex < 4)
            {
                playerPositionIndex++;
            }          
            Debug.Log("Moved Right and Index is --> " + playerPositionIndex);
            Debug.Log("Player position --> " + transform.position.x);
            //Debug.Log("Array value --> " + (float)playerPosition[playerPositionIndex + 1]);
        }
    }

    private void swipeLeftPosition()
    {
        if (transform.position.x <= playerPosition[playerPositionIndex - 1])
        {
            Debug.Log("Index before movement is --> " + playerPositionIndex);
            stopMovement();
            if (playerPositionIndex > 0)
            {
                playerPositionIndex--;
            }           
            Debug.Log("Moved Left and Index is --> " + playerPositionIndex);
            Debug.Log("Player position --> " + transform.position.x);
            //Debug.Log("Array value --> " + (float)playerPosition[playerPositionIndex - 1]);
        }
    }

    private void swipeCheck()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                swipeStartTime = Time.time;
                startSwipePosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                swipeEndTime = Time.time;
                endSwipePosition = touch.position;
                swipeTime = swipeEndTime - swipeStartTime;
                swipeLength = (endSwipePosition - startSwipePosition).magnitude;
                if (swipeTime < maxSwipeTime && swipeLength > minSwipeDistance)
                {
                    swipeControl();
                }
            }
        }
    }

    private void swipeControl()
    {
        Vector2 distance = endSwipePosition - startSwipePosition;
        if (endSwipePosition.x > startSwipePosition.x && playerPositionIndex < 4)
        {
            moveRightAnim();
        }
        else if (endSwipePosition.x < startSwipePosition.x && playerPositionIndex > 0)
        {
            moveLeftAnim();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision tag is something you want to react to.
        if (collision.gameObject.CompareTag("MineTile"))
        {
            // Stop the Rigidbody2D when a collision occurs.
            stopMovement();
            Debug.Log("Player has collided with an object !!");
        }
    }

    // Stop all movement and animations
    public void stopMovement()
    {
        isLeft = false;
        isRight = false;
        playerRB.velocity = Vector2.zero;
        animator.SetBool("Right", false);
        animator.SetBool("Left", false);
        animator.SetBool("Side_Pick_Left", false);
        animator.SetBool("Side_Pick_Right", false);
        animator.SetBool("Down_Pick_Left", false);
        animator.SetBool("Down_Pick_Right", false);
    }

    public void moveRightAnim()
    {
        isLeft = false;
        isRight = true;
        animator.SetBool("Left", false);
        animator.SetBool("Right", true);
        Debug.Log("Move Right");
    }

    public void moveLeftAnim()
    {
        isRight = false;
        isLeft = true;
        animator.SetBool("Right", false);
        animator.SetBool("Left", true);
        Debug.Log("Move Left");
    }

    public void moveRight()
    {
        isLeft = false;
        playerRB.velocity = new Vector2(1 * speed * Time.deltaTime, playerRB.velocity.y);
        swipeRightPosition();
        moveRightAnim();
        Debug.Log("Player should move RIGHT after mining !");
    }

    public void moveLeft()
    {
        isRight = false;
        playerRB.velocity = new Vector2(-1 * speed * Time.deltaTime, playerRB.velocity.y);
        swipeLeftPosition();
        moveLeftAnim();
        Debug.Log("Player should move LEFT after mining !");
    }
}
