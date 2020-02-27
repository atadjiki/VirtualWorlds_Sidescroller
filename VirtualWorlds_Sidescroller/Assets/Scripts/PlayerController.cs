using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    public Animator animator;
    private string IdleCrouchString = "IdleCrouch";
    private string RunCrouchString = "RunCrouch";
    private string IdleStandString = "IdleStand";
    private string RunStandString = "RunStand";
    private string JumpString = "Jumping";
    private string InteractString = "Interact";
    

    private Rigidbody rb;
    public GameObject body;
    public float speed = 0.05f;

    private bool isCrouching;
    private bool isRunning;
    private bool isJumping;

    private float jumpTime;

    public enum Direction { Left, Right, None };
    public enum PlayerAnimation { IdleStand, IdleCrouch, RunStand, RunCrouch, Jump, Interact };

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        foreach(AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if(clip.name == JumpString)
            {
                jumpTime = clip.length;
                break;
            }
        }

        isCrouching = false;
        isRunning = false;
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isJumping == false)
        {
            if (Input.GetKey(KeyCode.A))
            {
                isRunning = true;
                MoveDirection(Direction.Left);

            }
            else if (Input.GetKey(KeyCode.D))
            {
                isRunning = true;
                MoveDirection(Direction.Right);
            }
            else
            {
                isRunning = false;
                MoveDirection(Direction.None);
            }

            if (Input.GetKey(KeyCode.S))
            {
                isCrouching = true;
            }
            else
            {
                isCrouching = false;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                isJumping = true;
                SetAnimation(PlayerAnimation.Jump, false);
                StartCoroutine(JumpCooldown());
            }
        }
        
        
    }

    private void MoveDirection(Direction direction)
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

            float modifier = 1;

            if (direction == Direction.Left)
            {
                modifier = -1;
            }

            body.transform.eulerAngles = new Vector3(0, modifier * 90, 0);
            rb.MovePosition(new Vector3(body.transform.position.x + (modifier * speed), body.transform.position.y, body.transform.position.z));
            
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

    private  void SetAnimation(PlayerAnimation animation, bool reset)
    {
        if(reset)
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
        else if(animation == PlayerAnimation.RunStand)
        {
            animator.SetTrigger(RunStandString);
        }
        else if(animation == PlayerAnimation.Jump)
        {
            animator.SetBool(JumpString, isJumping);
        }
        else if(animation == PlayerAnimation.Interact)
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
        yield return new WaitForSeconds(jumpTime);
        isJumping = false;
    }
}
