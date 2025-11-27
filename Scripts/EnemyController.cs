using UnityEngine;
using UnityEngine.AI;

public enum PatrolType
{
    Patroller,
    Guard
}
public enum ActionType
{
    Patrol,
    Stand,
    Trace,
    Attack
}

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField]protected float actionTime;
    [SerializeField] protected ActionType actionType;
    [SerializeField]protected PatrolType patrolType;
    [SerializeField] protected Transform target;
    [SerializeField] protected Transform player;
    [SerializeField] protected float findDistance;
    [SerializeField] protected float traceDistance;
    [SerializeField] protected float attackDistance;
    [SerializeField] protected float alertDistance;
    [SerializeField] LayerMask playerLayer;

    [SerializeField] protected float attackCdSet;
    [SerializeField] protected float attackTime;
    [SerializeField] protected float attackCd;

    [SerializeField] protected LayerMask ground;
    protected Animator animator;

    protected AttributesController ac;
    protected StatusController stc;
    protected Rigidbody2D rb;

    protected string currentAnimation;
    protected bool facingRight = true;
    

    protected abstract void AnimationController();
    protected void SetAnimation(string anim, float fadeTime = 0.2f)
    {
        if(currentAnimation != anim)
        {
            currentAnimation = anim;
            if(animator == null)
            {
                animator = GetComponent<Animator>();
            }
            animator.CrossFade(anim, fadeTime);
        }
    }
    protected void ActionSelector()
    {
        if(actionTime > 0)
        {
            actionTime -=Time.deltaTime;
            switch (actionType)
            {
                case ActionType.Patrol:
                    Patrol();
                    break;
                case ActionType.Stand:
                    Stand();
                    break;
                case ActionType.Trace:
                    Trace();
                    break;
                case ActionType.Attack:
                    Attack();
                    break;
            }
        }
        else
        {
            if (target != null)
            {
                if (Vector2.Distance(transform.position, target.position) > attackDistance)
                {
                    actionType = ActionType.Trace;
                    actionTime = 1;
                }
                else
                {
                    actionType = ActionType.Attack;
                    actionTime = 1;
                }
                return;
            }
            switch (patrolType)
            {
                case PatrolType.Patroller:
                    actionType = (ActionType)(int)Mathf.Round(Random.Range(0,2));
                    if(actionType == ActionType.Stand)
                    {
                        actionTime = Random.Range(1, 3);
                    }
                    else if(actionType == ActionType.Patrol)
                    {
                        actionTime = Random.Range(1, 3);
                        facingRight = Random.Range(0, 2) >= 1 ? true : false;
                    }
                    break;
                case PatrolType.Guard:
                    actionType = ActionType.Stand;
                    break;
            }
        }
    }
    protected virtual void Stand()
    {
        if (target != null)
        {
            actionType = ActionType.Trace;
            rb.linearVelocityX = 0;
            actionTime = 1;
        }
        else
        {
            rb.linearVelocityX = 0;
        }
    }
    protected virtual void SetUp()
    {
        ac = GetComponent<AttributesController>();
        stc = GetComponent<StatusController>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void Trace()
    {
        if (target == null) return;
        if (Vector2.Distance(transform.position, target.position) <= attackDistance)
        {
            actionType = ActionType.Attack;
            rb.linearVelocityX = 0;
            actionTime = 1;
            return;
        }
        if (Vector2.Distance(transform.position, target.position) > traceDistance)
        {
            target = null;
        }
        if (target != null)
        {
            if(target.position.x > transform.position.x && Mathf.Abs(target.position.x - transform.position.x) > 0.5)
            {
                facingRight = true;
                Walk();
            }
            else if(target.position.x < transform.position.x && Mathf.Abs(target.position.x - transform.position.x) > 0.5)
            {
                facingRight = false;
                Walk();
            }
            else
            {
                if (rb != null)
                {
                    rb.linearVelocityX = 0;
                }
            }
            actionTime = 5;
        }
        else
        {
            actionTime = 0;
        }
    }
    protected void Patrol()
    {
        if(target != null)
        {
            actionType = ActionType.Trace;
            rb.linearVelocityX = 0;
            actionTime = 1;
        }
        else
        {
            if(Physics2D.Raycast((facingRight?Vector3.right:Vector3.left) + transform.position, Vector2.down, 1, ground) 
                && Physics2D.Raycast((facingRight ? Vector3.right : Vector3.left) + transform.position, (facingRight ? Vector3.right : Vector3.left), 0.5f, ground))
            {
                Walk();
            }
            else
            {
                actionTime = 0;
                rb.linearVelocityX = 0;
            }
            
            Debug.Log("HI");
        }
    }

    protected void Walk()
    {
        if (rb == null) return;
        if (facingRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            rb.linearVelocityX = ac.GetSpeed();
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            rb.linearVelocityX = -ac.GetSpeed();
        }
    }
    protected void PlayerFinder()
    {
        if(player != null && target == null)
        {
            if(Vector2.Distance(transform.position, player.position) < findDistance)
            {
                Vector2 temp = player.position - transform.position;
                Debug.Log(temp);
                if(Physics2D.Raycast(transform.position, temp.normalized, findDistance, playerLayer))
                {
                    target = player;
                    actionTime = 0;
                }
            }
        }
    }

    protected abstract void Attack();
}
