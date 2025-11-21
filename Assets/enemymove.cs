using UnityEngine;

public class enemymove : MonoBehaviour
{
    // === 公開設定 ===
    [Header("追跡対象")]
    public Transform player; // プレイヤーのTransformコンポーネント

    [Header("移動設定")]
    public float moveSpeed = 3f; // 移動速度
    public float jumpForce = 8f; // ジャンプ力
    public float distanceToJump = 2f; // ジャンプする距離

    [Header("地面判定")]
    public LayerMask groundLayer;    // 地面専用レイヤー
    public float groundCheckDistance = 0.1f; // 判定距離（少し短く）

    // === プライベート変数 ===
    private Rigidbody2D rb;
    private bool isGrounded;
    private float direction;
    private BoxCollider2D boxCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // ヒエラルキーから"Player"タグのオブジェクトを探してplayer変数に設定
        if (player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // プレイヤーが存在しない場合は処理を終了
        if (player == null)
        {
            return;
        }

        // プレイヤーとの水平方向の距離を計算
        float distanceX = player.position.x - transform.position.x;
        direction = Mathf.Sign(distanceX);

        // 敵とプレイヤーが一定距離離れていて、かつ地面に接している場合にジャンプ
        if (Mathf.Abs(distanceX) > distanceToJump && isGrounded)
        {
            JumpTowardsPlayer();
        }
    }

    void FixedUpdate()
    {
        // 地面チェック（レイキャストを使用）
        isGrounded = IsGrounded();

        // プレイヤーの方向へ移動
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
    }

    // プレイヤーに向かってジャンプするメソッド
    void JumpTowardsPlayer()
    {
        rb.AddForce(new Vector2(direction * moveSpeed, jumpForce), ForceMode2D.Impulse);
    }
    private bool IsGrounded()
    {
        Bounds bounds = boxCollider.bounds;
        Vector2 origin = new Vector2(bounds.center.x, bounds.min.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);

        // デバッグ可視化（シーンビューで確認できる）
        Debug.DrawRay(origin, Vector2.down * groundCheckDistance, hit.collider ? Color.green : Color.red);

        return hit.collider != null;
    }
}
