using UnityEngine;

public class MoveController : MonoBehaviour
{
    private Rigidbody2D rb;
    private AttributesController ac;
    private StatusController stc;
    private bool isMoving;
    private bool isFacingRight;
    private bool canMove;

    
    public void SetFacing(bool facing)
    {
        this.isFacingRight = facing;
    }
    public bool GetFacing()
    {
        return this.isFacingRight;
    }
    public void SetMove(bool isMoving)
    {
        this.isMoving = isMoving;
    }
    private void Move()
    {
        canMove = stc.CanMove();
        if (!canMove) return;
        if(isMoving)
        {
            if(isFacingRight)
            {
                rb.linearVelocityX = ac.GetSpeed() * Time.fixedDeltaTime;
            }
            else
            {
                rb.linearVelocityX = -ac.GetSpeed() * Time.fixedDeltaTime;
            }
        }
        else
        {
            rb.linearVelocityX = 0;
        }
    }

    

    void FixedUpdate()
    {
        Move();
    }

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ac = GetComponent<AttributesController>();
        stc = GetComponent<StatusController>();
    }
}
