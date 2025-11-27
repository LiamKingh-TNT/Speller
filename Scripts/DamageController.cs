using UnityEngine;

public class DamageController : MonoBehaviour
{
    private GameObject master;
    [SerializeField]private float destroyTime = 1f;
    [SerializeField] private float damage = 1;
    [SerializeField] private bool isPlayerAttack;
    [SerializeField] private bool isSingleTargetDamage;
    private Vector2 knockback = Vector2.up;
    private float stunnedTime = 0.5f;

    public void SetUp(GameObject _master, float _destroyTime, float _damage, Vector2 _knockback, float _stunnedTime)
    {
        master = _master;
        destroyTime = _destroyTime;
        damage = _damage;
        knockback = _knockback;
        stunnedTime = _stunnedTime;
        Debug.Log(knockback);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !isPlayerAttack)
        {
            collision.GetComponent<AttributesController>().OnHealthChanging(master, damage, stunnedTime, knockback);
        }
        if (collision.tag == "Enemy" && isPlayerAttack)
        {
            collision.GetComponent<AttributesController>().OnHealthChanging(master, damage, stunnedTime, knockback);
        }
        if(isSingleTargetDamage)
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        master = gameObject;
        if(master.tag == "Player")
        {
            isPlayerAttack = true;
        }
        else
        {
            isPlayerAttack = false;
        }
    }
    public void Update()
    {
        if(destroyTime < 0)
        {
            Destroy(gameObject);
        }
        destroyTime -= Time.deltaTime;
    }
}
