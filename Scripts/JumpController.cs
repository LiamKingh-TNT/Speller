using UnityEngine;

public class JumpController : MonoBehaviour
{
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask wall;
    [SerializeField] Transform groundChecker;
    private Rigidbody2D rb;
    private AttributesController ac;
    private bool isOnGround;
    private bool isJumpping;
    private float jumppedTime;
    private Animator animator;
    public void Jump()
    {
        int jumpCounts = ac.GetJumpCounts();
        if (jumpCounts > 0)
        {
            jumppedTime = 0.1f;
            isJumpping = true;
            rb.linearVelocityY = ac.GetJump();
            ac.SetJumpCounts(jumpCounts - 1);
        }
    }
    public bool GetIsJumpping()
    {
        return isJumpping;
    }
    private void GroundChecker()
    {
        isOnGround = Physics2D.Raycast(groundChecker.position, Vector2.down, 0.05f, ground) || Physics2D.Raycast(groundChecker.position, Vector2.down, 0.05f, wall);
        if(isOnGround && jumppedTime < 0)
        {
            isJumpping = false;
            ac.SetJumpCounts(ac.GetJumpCountsMax());
        }
        if(jumppedTime > 0)
        {
            jumppedTime -= Time.deltaTime;
        }
    }

    public bool GetOnGround()
    {
        return isOnGround;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ac = GetComponent<AttributesController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        GroundChecker();
    }
}
