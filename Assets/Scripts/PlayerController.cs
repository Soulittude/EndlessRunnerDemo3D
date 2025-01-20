using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;
    public float maxSpeed;

    private int desiredLane = 1; // 0 = Left, 1 = Middle, 2 = Right
    public float laneDistance = 4f; // The distance between two lanes

    public float jumpForce;
    public float gravity = -20;
    private bool isSliding = false;

    public Animator animator;

    private float groundCheckDelay = 0.1f; // Delay to confirm grounding
    private float lastGroundedTime;       // Tracks the last time the player was grounded

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!PlayerManager.isGameStarted)
            return;

        if(forwardSpeed<maxSpeed)
            forwardSpeed += 0.1f * Time.deltaTime;

        animator.SetBool("isGameStarted", true);

        // Forward movement
        direction.z = forwardSpeed;

        // Apply gravity
        if (!controller.isGrounded)
        {
            direction.y += gravity * Time.deltaTime;
        }
        else
        {
            direction.y = -0.1f; // Maintain grounding
            lastGroundedTime = Time.time; // Update grounded time
        }

        // Smooth ground check to avoid false ungrounding
        bool isSmoothlyGrounded = Time.time - lastGroundedTime <= groundCheckDelay;
        animator.SetBool("isGrounded", isSmoothlyGrounded);

        // Jump logic
        if (isSmoothlyGrounded && SwipeManager.swipeUp)
        {
            Jump();
        }

        if(SwipeManager.swipeDown && !isSliding)
        {
            StartCoroutine(Slide());
        }

        if(SwipeManager.swipeDown && !isSmoothlyGrounded)
        {
            direction.y = -jumpForce;;
        }

        // Handle lane changes
        HandleLaneChanges();

        // Calculate target position based on lane
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        }
        else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        // Smoothly move towards the target lane
        MoveTowardsTarget(targetPosition);
    }

    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted)
            return;

        // Apply movement
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void Jump()
    {
        direction.y = jumpForce; // Apply jump force
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;

        yield return new WaitForSeconds(1f);

        animator.SetBool("isSliding", false);
        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;
        isSliding = false;
    }

    private void HandleLaneChanges()
    {
        if (SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }

        if (SwipeManager.swipeLeft)
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }
    }

    private void MoveTowardsTarget(Vector3 targetPosition)
    {
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 * Time.deltaTime;

        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
        {
            controller.Move(moveDir);
        }
        else
        {
            controller.Move(diff);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            PlayerManager.gameOver = true;
        }
    }
}
