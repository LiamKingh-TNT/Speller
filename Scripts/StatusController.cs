using UnityEngine;

public class StatusController : MonoBehaviour
{
    private bool isStunned;
    private float stunnedTime;
    private bool isFrosen;
    private bool isAttacking;

    private void Update()
    {
        if(stunnedTime >= 0)
        {
            stunnedTime -= Time.deltaTime;
            isStunned = true;
        }
        else
        {
            isStunned = false;
        }
    }

    public void SetStunned(float _stunnedTime)
    {
        stunnedTime = _stunnedTime;
    }
    public void SetAttacking(bool isAttacking)
    {
        this.isAttacking = isAttacking;
    }

    public bool CanMove()
    {
        return (!isStunned && !isFrosen && !isAttacking);
    }

    public bool CanJump()
    {
        return (!isStunned && !isFrosen && !isAttacking);
    }

    public bool CanAttack()
    {
        return (!isStunned && !isFrosen);
    }
}
