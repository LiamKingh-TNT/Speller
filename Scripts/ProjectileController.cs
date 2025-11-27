using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public enum ProjectileType 
    { 
        Direct,
        Trace
    }

    public ProjectileType type = ProjectileType.Direct;
    private Vector2 direction = Vector2.right;
    [SerializeField]private float speed = 1f;
    private Rigidbody2D rb;

    public void SetUp(Vector2 d, float _speed)
    {
        direction = d;
        speed = _speed;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction * speed * Time.fixedDeltaTime;
    }
}
