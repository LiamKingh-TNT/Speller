using UnityEngine;

public class GoblinController : EnemyController
{
    
    void Start()
    {
        SetUp();
    }

    void Update()
    {
        attackTime -= Time.deltaTime;
        PlayerFinder();
        ActionSelector();
        AnimationController();
    }
    protected virtual void SetUp()
    {
        ac = GetComponent<AttributesController>();
        stc = GetComponent<StatusController>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    protected override void Attack()
    {

    }
    protected override void AnimationController()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            return;
        }
        if (Mathf.Abs(rb.linearVelocityX) > 0)
        {
            SetAnimation("GoblinWalk", 0.2f);
        }
        else
        {
            SetAnimation("GoblinIdle", 0.2f);
        }
    }
}
