using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;

    private int desiredLane = 1; // 0 = Left, 1 = Middle, 2 = Right
    public float laneDistance = 4f; // The distance between two lanes

    public float jumpForce;
    public float gravity = -20;

    public Animator animator;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }


    void Update()
    {

        if(!PlayerManager.isGameStarted)
            return;

        animator.SetBool("isGameStarted", true);
        direction.z = forwardSpeed;
        direction.y += gravity * Time.deltaTime; //comment olabilir
        
        animator.SetBool("isGrounded", controller.isGrounded);
        if(controller.isGrounded)
        {
            if(SwipeManager.swipeUp)
            {
                Jump();
            }
        }

        if(SwipeManager.swipeRight)
        {
            desiredLane++;
            if(desiredLane == 3)
            {
                desiredLane = 2;
            }
        }

        if(SwipeManager.swipeLeft)
        {
            desiredLane--;
            if(desiredLane == -1)
            {
                desiredLane = 0;
            }
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
    
        if(desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if(desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        //transform.position = Vector3.Lerp(transform.position, targetPosition, 80 * Time.fixedDeltaTime);
        if(transform.position == targetPosition)
        {
            return;
        }
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;
        if(moveDir.sqrMagnitude < diff.sqrMagnitude)
        {
            controller.Move(moveDir);
        }
        else
        {
            controller.Move(diff);
        }
    }

    private void FixedUpdate()
    {
        if(!PlayerManager.isGameStarted)
            return;
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
        }
    }
}
