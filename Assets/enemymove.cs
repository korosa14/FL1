using UnityEngine;
using UnityEngine.Events;

public class enemymove : MonoBehaviour
{
    // === 公開設定 ===
    [Header("追跡対象")]
    public Transform player; // プレイヤーのTransformコンポーネント

    [Header("移動設定")]
    public float moveSpeed = 0.5f; // 移動速度
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

    private bool inv;

    private bool fall;

  

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

        inv=false;
        fall=false;
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

        if(rb.transform.position.y<=-10){
            fall=true;
        }

    }

    void FixedUpdate()
    {
        // 地面チェック（レイキャストを使用）
        isGrounded = IsGrounded();

        if(!inv){
             // プレイヤーの方向へ移動
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        }
       
    }

    // プレイヤーに向かってジャンプするメソッド
    private bool IsGrounded()
    {
        Bounds bounds = boxCollider.bounds;
        Vector2 origin = new Vector2(bounds.center.x, bounds.min.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);

        // デバッグ可視化（シーンビューで確認できる）
        Debug.DrawRay(origin, Vector2.down * groundCheckDistance, hit.collider ? Color.green : Color.red);

        return hit.collider != null;
    }

    public void SetInv(bool flag){
        inv=flag;
    }
    public bool GetInv(){
        return inv;
    }
     // 攻撃を食らった時に外部から呼ぶメソッド
    public void PlayKnockback(float power)
    {
        // 1. 敵の場所から自分の場所への方向を計算
        Vector2 knockbackDir = ((Vector2)transform.position - new Vector2(player.transform.position.x,player.transform.position.y)).normalized;

        // 2. 現在の速度を一度ゼロにする（連続ヒット時の不自然な挙動を防ぐ）
        rb.linearVelocity = Vector2.zero;

        // 3. 反対方向へ力を加える (Impulseは瞬間的な力)
        rb.AddForce(knockbackDir * power, ForceMode2D.Impulse);
    }
    public bool Getfall(){
        return fall;
    }

}
