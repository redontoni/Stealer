using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTap : MonoBehaviour
{
    private PlayerController playerController;
    private Transform transformPlayer;
    private Transform transform;
    private Animator animator;
    private AnimationClip animationClipPickLeft;
    private AnimationClip animationClipPickRight;
    private AnimationClip animationToControlPickDownLeft;
    private AnimationClip animationToControlPickDownRight;
    private SpriteRenderer spriteRendererPlayer;
    private BoxCollider2D boxCollider2;

    private float lastTapTime;
    private float doubleTapDelay = 0.2f; // Adjust this value to your desired double-tap speed.

    private int animationCycleCount = 0;

    private bool isRight = false;
    private bool isLeft = false;
    private bool isUp = false;

    public bool canMine = false;
    void Start()
    {
        playerController = Singleton.instance.playerController;

        transform = GetComponent<Transform>();
        boxCollider2 = GetComponent<BoxCollider2D>();
        transformPlayer = Singleton.instance.player.GetComponent<Transform>();

        animator = Singleton.instance.playerController.animator;
        animationClipPickRight = Singleton.instance.animationToControlPickRight;
        animationClipPickLeft = Singleton.instance.animationToControlPickLeft;
        animationToControlPickDownLeft = Singleton.instance.animationToControlPickDownLeft;
        animationToControlPickDownRight = Singleton.instance.animationToControlPickDownRight;

        spriteRendererPlayer = transformPlayer.GetComponent<SpriteRenderer>();
        boxCollider2.isTrigger = true;
    }

    private void Update()
    {
        // Check if the specific animation has completed two cycles
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationClipPickLeft.name) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 2.0f)
        {
            animationControlCount();
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationClipPickRight.name) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 2.0f)
        {
            animationControlCount();
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationToControlPickDownLeft.name) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 2.0f)
        {
            animationControlCount();
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName(animationToControlPickDownRight.name) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 2.0f)
        {
            animationControlCount();
        }
    }

    public void animationControlCount() {
        animationCycleCount++;

        // Stop the specific animation after 2 cycles
        if (animationCycleCount >= 2)
        {
            playerController.stopMovement();
            animationCycleCount = 0;

            // Set Mine tile new state
            if (isLeft)
            {
                mineTileStatesLeft();
                isLeft = false;
                isRight = false;
                isUp = false;
            }
            else if (isRight)
            {
                mineTileStatesRight();
                isLeft = false;
                isRight = false;
                isUp = false;
            }
            else if (isUp)
            {
                mineTileStatesUp();
                isLeft = false;
                isRight = false;
                isUp = false;
            }
        }
    }

    public void OnMouseDown()
    {
       if (canMine)
        {
            // Check if the time between two clicks is less than the double tap delay.
            if (Time.time - lastTapTime < doubleTapDelay)
            {
                // Double tap detected, trigger your event here.
                Debug.Log("Double Tap Detected!");
                // Add your event code here.
                playerSidePick();
            }
            else
            {
                // Store the time of this tap for future comparison.
                lastTapTime = Time.time;
            }
        }
       
    }

    // Check if Player is right or left
    private void playerSidePick()
    {
        float transPlayer = transformPlayer.position.y;
        float transTile = transform.position.y;
        float yDifference = transPlayer - transTile;
        Debug.Log("Y of Player is --> " + transPlayer);
        Debug.Log("Y of MineTile clicked is --> " + transTile);
        Debug.Log("Difference of Y is --> " + yDifference);

        if (transformPlayer.position.x > transform.position.x && yDifference >= 0 && yDifference < 1)
        {
            playerController.stopMovement();
            animator.SetBool("Side_Pick_Left", true);
            Debug.Log("Player is in the Left !! ");
            isLeft = true;
            isRight = false;
            isUp = false;
        }
        else if (transformPlayer.position.x < transform.position.x && yDifference >= 0 && yDifference < 1)
        {
            playerController.stopMovement();
            animator.SetBool("Side_Pick_Right", true);
            Debug.Log("Player is in the Right !! ");
            isLeft = false;
            isRight = true;
            isUp = false;
        }
        else if (yDifference < 0 || yDifference > 1 && spriteRendererPlayer.flipX == true)
        {
            playerController.stopMovement();
            animator.SetBool("Down_Pick_Right", true);
            Debug.Log("Player is up and Idle position Right !! ");
            isLeft = false;
            isRight = false;
            isUp = true;
        }
        else if (yDifference < 0 || yDifference > 1 && spriteRendererPlayer.flipX == false)
        {
            playerController.stopMovement();
            animator.SetBool("Down_Pick_Left", true);
            Debug.Log("Player is up and Idle position Left !! ");
            isLeft = false;
            isRight = false;
            isUp = true;
        }
    }

    private void mineTileStatesLeft()
    {
        Transform fullMineTile = transform.Find("fullMineTile");
        Transform twoThirdMineTileLeft = transform.Find("twoThirdMineTileLeft");
        Transform oneThirdMineTileLeft = transform.Find("oneThirdMineTileLeft");

        if (fullMineTile.gameObject.activeSelf)
        {
            fullMineTile.gameObject.SetActive(false);
            twoThirdMineTileLeft.gameObject.SetActive(true);
            twoThirdMineTileLeft.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            twoThirdMineTileLeft.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;

            playerController.moveLeft();
        }
        else if (twoThirdMineTileLeft.gameObject.activeSelf)
        {
            twoThirdMineTileLeft.gameObject.SetActive(false);
            oneThirdMineTileLeft.gameObject.SetActive(true);
            oneThirdMineTileLeft.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            oneThirdMineTileLeft.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;

            playerController.moveLeft();
        } 
        else
        {
            transform.gameObject.SetActive(false);

            playerController.moveLeft();
        }
    }

    private void mineTileStatesRight()
    {
        Transform fullMineTile = transform.Find("fullMineTile");
        Transform twoThirdMineTileRight = transform.Find("twoThirdMineTileRight");
        Transform oneThirdMineTileRight = transform.Find("oneThirdMineTileRight");

        if (fullMineTile.gameObject.activeSelf)
        {
            fullMineTile.gameObject.SetActive(false);
            twoThirdMineTileRight.gameObject.SetActive(true);
            twoThirdMineTileRight.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            twoThirdMineTileRight.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;

            playerController.moveRight();
        }
        else if (twoThirdMineTileRight.gameObject.activeSelf)
        {
            twoThirdMineTileRight.gameObject.SetActive(false);
            oneThirdMineTileRight.gameObject.SetActive(true);
            oneThirdMineTileRight.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            oneThirdMineTileRight.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;

            playerController.moveRight();
        }
        else
        {
            transform.gameObject.SetActive(false);

            playerController.moveRight();
        }
    }

    private void mineTileStatesUp()
    {
        Transform fullMineTile = transform.Find("fullMineTile");
        Transform twoThirdMineTileUp = transform.Find("twoThirdMineTileUp");
        Transform oneThirdMineTileUp = transform.Find("oneThirdMineTileUp");

        if (fullMineTile.gameObject.activeSelf)
        {
            fullMineTile.gameObject.SetActive(false);
            twoThirdMineTileUp.gameObject.SetActive(true);
            twoThirdMineTileUp.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            twoThirdMineTileUp.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        else if (twoThirdMineTileUp.gameObject.activeSelf)
        {
            twoThirdMineTileUp.gameObject.SetActive(false);
            oneThirdMineTileUp.gameObject.SetActive(true);
            oneThirdMineTileUp.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            oneThirdMineTileUp.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        else
        {
            transform.gameObject.SetActive(false);
        }
    }
}
