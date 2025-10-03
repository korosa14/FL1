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
    public float jumpForce = 10f;     // ジャンプ力

    [Header("地面判定")]
    public LayerMask groundLayer;    // 地面専用レイヤー

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private float moveInput;         // 左右入力
    private float currentSpeed;
    private bool canJump = false;    // 地面接触時のみジャンプ可能
    void Start()
    {
        
    }

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

        // ジャンプ
        if (Keyboard.current.spaceKey.wasPressedThisFrame && canJump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false; // ジャンプ直後は空中扱い
        }
    }

    void FixedUpdate()
    {
        // X方向移動のみ velocity に反映、Y方向はジャンプに任せる
        Vector2 vel = rb.linearVelocity;
        vel.x = moveInput * currentSpeed;
        rb.linearVelocity = vel;
    }

    // 地面に接触した瞬間にジャンプ可能
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            canJump = true;
        }
    }

    // 地面から離れた瞬間にジャンプ不可
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            canJump = false;
        }
    }
}