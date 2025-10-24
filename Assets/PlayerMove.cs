using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMove : MonoBehaviour
{
    [Header("移動速度")]
    public float normalSpeed = 5f;   // 通常速度
    public float dashSpeed = 8f;     // ダッシュ速度
    public float slowSpeed = 2f;     // スロー速度

    [Header("ジャンプ設定")]
    public float jumpForce = 10f;    // ジャンプ力

    [Header("地面判定")]
    public LayerMask groundLayer;    // 地面専用レイヤー
    public float groundCheckDistance = 0.1f; // 判定距離（少し短く）

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private float moveInput;         // 左右入力
    private float currentSpeed;
    private bool canJump = false;    // 地面接触時のみジャンプ可能


    private bool facingRight = true;

    [SerializeField] private int HP = 20;

    [SerializeField] private bool dethFlag=false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.freezeRotation = true;

        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // 左右入力
        moveInput = 0f;
        if (Keyboard.current.aKey.isPressed) moveInput = -1f;
        if (Keyboard.current.dKey.isPressed) moveInput = 1f;

        // スピード切り替え
        if (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed)
            currentSpeed = dashSpeed;
        else if (Keyboard.current.leftCtrlKey.isPressed || Keyboard.current.rightCtrlKey.isPressed)
            currentSpeed = slowSpeed;
        else
            currentSpeed = normalSpeed;

        // 接地判定を更新
        canJump = IsGrounded();

        // ジャンプ
        if (Keyboard.current.spaceKey.wasPressedThisFrame && canJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // 安定ジャンプ
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // 向き反転
        if (moveInput > 0 && !facingRight)
            Flip();
        else if (moveInput < 0 && facingRight)
            Flip();
    }

    void FixedUpdate()
    {
        // X方向移動のみ velocity に反映、Y方向はジャンプに任せる
        Vector2 vel = rb.linearVelocity;
        float targetX = moveInput * currentSpeed;

        //徐々に速度を近づける
        vel.x = Mathf.Lerp(vel.x, targetX, 0.15f);
        rb.linearVelocity = vel;
    }

    /// <summary>
    /// 接地判定（Raycast方式）
    /// </summary>
    private bool IsGrounded()
    {
        Bounds bounds = boxCollider.bounds;
        Vector2 origin = new Vector2(bounds.center.x, bounds.min.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);

        // デバッグ可視化（シーンビューで確認できる）
        Debug.DrawRay(origin, Vector2.down * groundCheckDistance, hit.collider ? Color.green : Color.red);

        return hit.collider != null;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public int GetHP()
    {
        return HP;
    }

    public bool GetdethFlag()
    {
        return dethFlag;
    }
}

