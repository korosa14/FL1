using UnityEngine;

public class enemymove : MonoBehaviour
{
    // �v���C���[��Transform��Inspector����ݒ�
    public GameObject player;

    // �G�̈ړ����x
    public float moveSpeed = 5f;

    // �W�����v�̗�
    public float jumpForce = 8f;

    // ���̃W�����v�܂ł̑ҋ@����
    public float jumpInterval = 2f;

    // �ڒn����̂��߂̕ϐ�
    public LayerMask groundLayer;
    public Transform groundCheck;
    private bool isGrounded;

    private Rigidbody2D rb;
    private float nextJumpTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        nextJumpTime = Time.time + jumpInterval;
    }

    void Update()
    {
        // �ڒn����
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // �W�����v�̃^�C�~���O���`�F�b�N
        if (isGrounded && Time.time >= nextJumpTime)
        {
            JumpTowardsPlayer();
            nextJumpTime = Time.time + jumpInterval;
        }
    }

    void JumpTowardsPlayer()
    {
        // �v���C���[�̕������v�Z
        Vector2 direction = (player.transform.position - transform.position).normalized;

        // X�����̑��x��ݒ�
        float horizontalVelocity = direction.x * moveSpeed;

        // �W�����v�̗͂�������
        Vector2 jumpVelocity = new Vector2(horizontalVelocity, jumpForce);
        rb.linearVelocity = jumpVelocity;
    }
}
