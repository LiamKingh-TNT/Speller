using UnityEngine;
using UnityEngine.AI;

public class BatController : EnemyController
{
    private bool attacked;
    protected NavMeshAgent agent;
    [SerializeField] private GameObject soundWave;
    [SerializeField] private Transform shooter;
    void Start()
    {
        SetUp();
    }

    protected override void Stand()
    {
        if (target != null)
        {
            actionType = ActionType.Trace;
            rb.linearVelocityX = 0;
            actionTime = 1;
        }
    }
    protected override void Trace()
    {
        Debug.Log("HI");
        if (target == null) return;
        if (Vector2.Distance(transform.position, target.position) <= attackDistance)
        {
            actionType = ActionType.Attack;
            actionTime = 1;
        }
        if (Vector2.Distance(transform.position, target.position) > traceDistance)
        {
            target = null;
        }
        if (target != null)
        {
            agent.SetDestination(target.position);
            agent.speed = ac.GetSpeed();
            if (target.position.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            actionTime = 5;
        }
        else
        {
            actionTime = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        attackTime -= Time.deltaTime;
        PlayerFinder();
        ActionSelector();
        AnimationController();
    }
    protected override void SetUp()
    {
        ac = GetComponent<AttributesController>();
        stc = GetComponent<StatusController>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.speed = ac.GetSpeed();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    protected override void AnimationController()
    {
        if (attackTime <= 0)
        {
            SetAnimation("BatSwing");
        }
    }
    protected override void Attack()
    {
        
        agent.SetDestination(transform.position);
        if (Vector2.Distance(transform.position, target.position) > attackDistance && attackTime < 0)
        {
            actionType = ActionType.Trace;
            actionTime = 1;
        }
        actionTime = 1;
        if(target != null)
        {
            if (target.position.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            if (attackCd <= 0)
            {

                attackCd = attackCdSet;
                attackTime = 0.8f;
                attacked = false;
                SetAnimation("BatAttack", 0);
            }
            else
            {
                attackCd -= Time.deltaTime;
            }

            if (attackTime < 0.3f && !attacked)
            {
                attacked = true;
                GameObject temp = Instantiate(soundWave);
                temp.transform.position = shooter.position;
                Vector2 dir = target.position - shooter.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                temp.transform.rotation = Quaternion.Euler(0, 0, angle);
                temp.GetComponent<DamageController>().SetUp(gameObject, 10, ac.GetAttackDamage(), new Vector2( (target.position.x > shooter.position.x? 1: -1), 1) * 5, 0.4f);
                temp.GetComponent<ProjectileController>().SetUp((target.position - shooter.position).normalized, 400);
            }
            
        }
    }
}
