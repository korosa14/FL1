using System.Collections;
using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    [Header("攻撃キー")]
    public KeyCode attackKey = KeyCode.J;

    [Header("攻撃設定")]
    public float startAngle = 90f;     // 構えの角度（上）
    public float endAngle = -45f;      // 振り下ろす角度（下）
    public float swingDuration = 0.15f; // 振る速さ
    public float returnDuration = 0.1f; // 元に戻す速さ

    private bool isSwinging = false;
    private Quaternion initialRotation;
    private Collider2D swordCollider;

    void Start()
    {
        swordCollider = GetComponentInChildren<Collider2D>();
        if (swordCollider != null)
            swordCollider.enabled = false;

        initialRotation = transform.localRotation;
    }

    void Update()
    {
        if (!isSwinging && Input.GetKeyDown(attackKey))
        {
            StartCoroutine(SwingDownRoutine());
        }
    }

    IEnumerator SwingDownRoutine()
    {
        isSwinging = true;

        // コライダーON
        if (swordCollider != null)
            swordCollider.enabled = true;

        float t = 0f;
        // 上から下へ振り下ろす
        while (t < swingDuration)
        {
            t += Time.deltaTime;
            float ratio = Mathf.Clamp01(t / swingDuration);
            float zAngle = Mathf.Lerp(startAngle, endAngle, ratio);
            transform.localRotation = initialRotation * Quaternion.Euler(0, 0, zAngle);
            yield return null;
        }

        // 少し間を置く（当たり判定維持）
        yield return new WaitForSeconds(0.05f);

        // コライダーOFF
        if (swordCollider != null)
            swordCollider.enabled = false;

        // 元の角度に戻す
        t = 0f;
        while (t < returnDuration)
        {
            t += Time.deltaTime;
            float ratio = Mathf.Clamp01(t / returnDuration);
            float zAngle = Mathf.Lerp(endAngle, startAngle, ratio);
            transform.localRotation = initialRotation * Quaternion.Euler(0, 0, zAngle);
            yield return null;
        }

        transform.localRotation = initialRotation;
        isSwinging = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isSwinging && other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit: " + other.name);
            // other.GetComponent<Enemy>()?.TakeDamage(1);
        }
    }
}
