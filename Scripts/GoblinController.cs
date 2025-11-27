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
    protected override void Attack()
    {

    }
    protected override void AnimationController()
    {
        
    }
}
