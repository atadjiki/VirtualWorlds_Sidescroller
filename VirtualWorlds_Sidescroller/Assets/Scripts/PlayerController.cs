using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    private static PlayerController _instance;

    public static PlayerController Instance { get { return _instance; } }

    public Animator animator;
    private string IdleCrouchString = "IdleCrouch";
    private string RunCrouchString = "RunCrouch";
    private string IdleStandString = "IdleStand";
    private string RunStandString = "RunStand";
    private string JumpString = "Jumping";
    private string InteractString = "Interact";

    private Rigidbody rb;
    private Collider col;
    public GameObject body;

    public float runSpeed = 0.05f;
    public float crouchSpeed = 0.02f;
    public float jumpDistance;

    private bool isCrouching;
    private bool isJumping;
    private bool isRunning;
    private bool isInteracting;

    private float interactTime;
    private float jumpTime;

    private float jumpForce;

    private float distToGround;
 

    public enum Direction { Up, Left, Right, None };
    public enum PlayerAnimation { IdleStand, IdleCrouch, RunStand, RunCrouch, Jump, Interact };

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        Build();
    }

    private void Build()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        distToGround = col.bounds.extents.y;

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == JumpString)
            {
                jumpTime = clip.length;

            }
            else if (clip.name == InteractString)
            {
                interactTime = clip.length;
            }
        }

        isCrouching = false;
        isJumping = false;
        isRunning = false;
        isInteracting = false;
    }

    private void FixedUpdate()
    {

        if (GameState.Instance.currentState == GameState.State.InGame)
        {
            if (isInteracting == false)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    MoveDirection(Direction.Left);
                    AnimationByDirection(Direction.Left);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    MoveDirection(Direction.Right);
                    AnimationByDirection(Direction.Right);
                }
                else
                {
                    AnimationByDirection(Direction.None);
                }

                if (isJumping == false)
                {
                    
                    if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.LeftShift))
                    {
                        isCrouching = true;
                    }
                    else
                    {
                        isCrouching = false;
                    }

                    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
                    {
                        StartCoroutine(JumpCooldown());
                    }
                }
            }
        }
    }

    private void AnimationByDirection(Direction direction)
    {
        if (direction == Direction.Left || direction == Direction.Right)
        {
            if (isCrouching)
            {
                SetAnimation(PlayerAnimation.RunCrouch, true);
            }
            else
            {
                SetAnimation(PlayerAnimation.RunStand, true);

            }
        }

        else if (direction == Direction.None)
        {
            if (isCrouching)
            {
                SetAnimation(PlayerAnimation.IdleCrouch, true);
            }
            else
            {
                SetAnimation(PlayerAnimation.IdleStand, true);
            }
        }
    }

    private Vector3 ValidateMoveVector(Vector3 desiredPosition)
    {
        Vector3 direction = desiredPosition - body.transform.position;
        Ray ray = new Ray(body.transform.position, direction);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, direction.magnitude))
        {
            return desiredPosition;
        }
        else
        {
            if (hit.collider.gameObject.GetComponent<Wall>())
            {
                return body.transform.position;
            }
            else if(hit.collider.gameObject.GetComponent<DetectionCollider>())
            {
                return desiredPosition;
            }
            else if (hit.collider.gameObject.GetComponent<Lever>())
            {
                return desiredPosition;
            }
            else
            {
                return hit.point;
            }
        }
    }

    private void MoveDirection(Direction direction)
    {
        if (direction == Direction.Left || direction == Direction.Right)
        {
            float modifier = 1;

            if (direction == Direction.Left)
            {
                modifier = -1;
            }

            body.transform.eulerAngles = new Vector3(0, modifier * 90, 0);

            if (isCrouching == false)
            {
                rb.MovePosition(ValidateMoveVector(new Vector3(body.transform.position.x + (modifier * runSpeed), body.transform.position.y, body.transform.position.z)));
            }
            else
            {
                rb.MovePosition(ValidateMoveVector(new Vector3(body.transform.position.x + (modifier * crouchSpeed), body.transform.position.y, body.transform.position.z)));
            }

            isRunning = true;

        }
        else if (direction == Direction.Up)
        {
            isRunning = false;
        }
    }

    private void SetAnimation(PlayerAnimation animation, bool reset)
    {
        if (reset)
        {
            ResetTriggers();
        }

        if (animation == PlayerAnimation.IdleCrouch)
        {
            animator.SetTrigger(IdleCrouchString);
        }
        else if (animation == PlayerAnimation.IdleStand)
        {
            animator.SetTrigger(IdleStandString);

        }
        else if (animation == PlayerAnimation.RunCrouch)
        {
            animator.SetTrigger(RunCrouchString);
        }
        else if (animation == PlayerAnimation.RunStand)
        {
            animator.SetTrigger(RunStandString);
        }
        else if (animation == PlayerAnimation.Jump)
        {
            animator.SetBool(JumpString, isJumping);
        }
        else if (animation == PlayerAnimation.Interact)
        {
            animator.SetTrigger(InteractString);
        }

    }

    private void ResetTriggers()
    {
        animator.ResetTrigger(IdleCrouchString);
        animator.ResetTrigger(IdleStandString);
        animator.ResetTrigger(RunCrouchString);
        animator.ResetTrigger(RunStandString);
        animator.ResetTrigger(JumpString);
        animator.ResetTrigger(InteractString);

    }

    IEnumerator JumpCooldown()
    {
        isJumping = true;
        MoveDirection(Direction.Up);
        SetAnimation(PlayerAnimation.Jump, false);
        yield return new WaitForSeconds(jumpTime);
        isJumping = false;
    }

   

    public void Interact(Lever lever)
    {
        StartCoroutine(DelayInteract(lever));
    }

    IEnumerator DelayInteract(Lever lever)
    {
        isInteracting = true;
        ResetTriggers();
        animator.Play(InteractString);
        yield return new WaitForSeconds(interactTime / 3);
        lever.LeverCallback();
        yield return new WaitForSeconds(2*interactTime / 3);
        animator.SetTrigger(IdleStandString);
        isInteracting = false;
    }

    public void Reset()
    {
        SetAnimation(PlayerAnimation.IdleStand, true);
    }

    public bool Jumping()
    {
        return isJumping;
    }

    public bool Crouching()
    {
        return isCrouching;
    }

    public bool Running()
    {
        return isRunning;
    }

    public bool Interacting()
    {
        return isInteracting;
    }
}
