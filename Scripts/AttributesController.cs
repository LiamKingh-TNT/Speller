using UnityEngine;


public class AttributesController : MonoBehaviour
{
    [SerializeField] private float healthBasic;
    private float healthMulti;
    private float healthMax;
    private float health;

    [SerializeField] private float speedBasic;
    private float speedMulti;
    private float speed;

    [SerializeField] private int jumpCountsBasic;
    private int jumpCountsMulti;
    private int jumpCountsMax;
    private int jumpCounts;

    [SerializeField] private float jumpBasic;
    private float jumpMulti;
    private float jump;

    [SerializeField] private float attackSpeedBasic;
    private float attackSpeedMulti;
    private float attackSpeed;

    [SerializeField] private float attackDamageBasic;
    private float attackDamageMulti;
    private float attackDamage;

    [SerializeField] private float skillCdSpeedBasic;
    private float skillCdSpeedMulti;
    private float skillCdSpeed;

    Rigidbody2D rb;
    StatusController stc;

    public void OnHealthChanging(object sender, float value)
    {
        Debug.Log($"[sender: [{sender}], Value: [{value}]");
    }
    public void OnHealthChanging(object sender, float value, float stunnedTIme, Vector2 _knockback)
    {
        Debug.Log($"[sender: [{sender}], Value: [{value}], Knockback: [{_knockback}]");
        stc.SetStunned(stunnedTIme);
        rb.linearVelocity = _knockback;
    }
    public void Start()
    {
        healthMax = healthBasic + healthMulti;
        health = healthMax;
        speed = speedBasic + speedMulti;
        jump = jumpBasic + jumpMulti;
        jumpCountsMax = jumpCountsBasic + jumpCountsMulti;
        jumpCounts = jumpCountsMax;
        attackSpeed = attackSpeedBasic + attackSpeedMulti;
        attackDamage = attackDamageBasic + attackDamageMulti;
        rb = GetComponent<Rigidbody2D>();
        stc = GetComponent<StatusController>();
    }

    
    public float GetHealth()
    {
        return health;
    }
    public float GetMaxHealth()
    {
        return healthMax;
    }
    public float GetSpeed()
    {
        return speed;
    }
    public float GetJump()
    {
        return jump;
    }
    public int GetJumpCounts()
    {
        return jumpCounts;
    }
    public int GetJumpCountsMax()
    {
        return jumpCountsMax;
    }
    public float GetAttackSpeed()
    {
        return attackSpeed;
    }
    public float GetAttackDamage()
    {
        return attackDamage;
    }

    public void SetHealthMulti(float value)
    {
        healthMulti = value;
        float temp = healthMax;
        healthMax = healthBasic + healthMulti;
        temp = healthMax - temp;
        health += temp;
    }
    public void SetJumpCounts(int jumpCounts)
    {
        this.jumpCounts = jumpCounts;
    }
}


