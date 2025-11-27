
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private StatusController stc;
    private MoveController mc;
    private JumpController jc;
    [SerializeField]private AttributesController ac;
    private Rigidbody2D rb;
    private Animator animator;

    private int attackStep;
    private float attackCd;
    private float attackStepCd;
    private bool attacking;
    private bool pressedAttack;
    private string currentAnimation;
    private float idleSwitchCD;
    private int currentIdle;

    public void JumpTrigger(InputAction.CallbackContext ctx)
    {
        if(stc.CanJump() && ctx.started)
        {
            jc.Jump();
        }
    }

    public void AttackTrigger(InputAction.CallbackContext ctx)
    {
        if (stc.CanAttack() && ctx.performed)
        {
            pressedAttack = true;
            //Debug.Log("atk");
        }
    }

    private void Attack()
    {
        SetAnimation("PlayerSwordAttack" + (attackStep+1).ToString(),0);
        
        attackStep++;
        if (attackStep > 1)
        {
            attackStep = 0;
        }

    }

    private void AttackResume()
    {
        if (pressedAttack && attackCd <= 0)
        {
            attackStepCd = 0.6f;
            attackCd = 0.45f;
            attacking = true;
            pressedAttack = false;
            Attack();
        }
        if (attackCd > 0)
        {
            attackCd -= Time.deltaTime;
            if(attackCd < 0.38)
            {
                if (attackStep == 0)
                {
                    rb.linearVelocityX = 2 * (mc.GetFacing() ? 1 : -1);
                }
                else
                {
                    rb.linearVelocityX *= 0.95f;
                }
            }
            if(attackCd < 0.15)
            {
                rb.linearVelocityX *= 0.95f;
            }
        }
        else
        {
            attacking = false;
        }
        if(attackStepCd > 0)
        {
            attackStepCd -= Time.deltaTime;
        }
        else
        {
            attackStep = 0;
        }
        stc.SetAttacking(attacking);
    }
    public void MoveTrigger(InputAction.CallbackContext ctx)
    {
        
        if (ctx.canceled)
        {
            mc.SetMove(false);
        }
        if (ctx.performed)
        {
            if (stc.CanMove())
            {
                mc.SetMove(true);
                if (ctx.ReadValue<float>() > 0)
                {
                    mc.SetFacing(true);
                }
                if (ctx.ReadValue<float>() < 0)
                {
                    mc.SetFacing(false);
                }
            }
        }
        
    }

    private void Start()
    {
        stc = GetComponent<StatusController>();
        mc = GetComponent<MoveController>();
        jc = GetComponent<JumpController>();
        rb = GetComponent<Rigidbody2D>();
        ac = GetComponent<AttributesController>();
    }
    private void Update()
    {
        AnimationController();
        AttackResume();
    }
    private void AnimationController()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
            return;
        }
        if(mc.GetFacing())
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0,180,0);
        }
        if (!attacking)
        {
            if(jc.GetOnGround())
            {
                if (Mathf.Abs(rb.linearVelocityX) < 0.1 && Mathf.Abs(rb.linearVelocityY) < 0.1)
                {
                    if(idleSwitchCD>=0)
                    {
                        idleSwitchCD -= Time.deltaTime;
                    }
                    else
                    {
                        currentIdle = (int)Mathf.Round(Random.Range(1, 5));
                        Debug.Log(currentIdle);
                        idleSwitchCD = 4;
                    }
                    SetAnimation("PlayerIdle" + currentIdle.ToString(), 0.1f);
                }
                if (Mathf.Abs(rb.linearVelocityX) > 0.1)
                {
                    SetAnimation("PlayerRun", 0.1f);
                }
            }
            else
            {
                if (rb.linearVelocityY > 0.1)
                {
                    if (ac.GetJumpCounts() == ac.GetJumpCountsMax() - 1)
                    {
                        SetAnimation("PlayerJump", 0);
                    }
                    else
                    {
                        SetAnimation("PlayerDoubleJump", 0);
                    }
                }
                else
                {
                    SetAnimation("PlayerFall", 0.2f);
                }
            }
            
        }
    }

    private void SetAnimation(string animation, float fadeTime = 0.2f)
    {
        if(animation != currentAnimation)
        {
            if(currentAnimation == null)
            {
                currentAnimation = animation;
            }
            if (currentAnimation.Contains("PlayerIdle"))
            {
                animator.CrossFade(animation, fadeTime / 4);
            }
            else
            {
                animator.CrossFade(animation, fadeTime);
            }
            currentAnimation = animation;
            //Debug.Log("anim:" + animation + "contain: " + animation.Contains("PlayerIdle"));
            
            
        }
    }
}
