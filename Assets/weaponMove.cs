using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SwordSwing : MonoBehaviour
{
    [Header("Swing settings")]
    public KeyCode swingKey = KeyCode.F; // �U��{�^��
    public float preWindTime = 0.05f;        // ���߁i�C�Ӂj
    public float swingDuration = 0.20f;      // �U�艺�낵����
    public float recoverDuration = 0.12f;    // ���Ƃɖ߂鎞��
    public float swingAngle = 200f;          // �U���p�x�ideg�j
    public AnimationCurve swingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Behavior")]
    public bool startsEnabled = false;       // �ʏ�̓R���C�_�[�����B�X�C���O�������L���ɂ���

    // internal
    Collider2D col;
    bool isSwinging = false;
    Quaternion initialLocalRot;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        col.enabled = startsEnabled;
        initialLocalRot = transform.localRotation;
        // Ensure pivot is set correctly in editor (or parent used as pivot)
    }

    void Update()
    {
        if (!isSwinging && Input.GetKeyDown(swingKey))
        {
            StartCoroutine(SwingRoutine());
        }
    }

    IEnumerator SwingRoutine()
    {
        isSwinging = true;

        // optional short wind-up
        if (preWindTime > 0f)
            yield return new WaitForSeconds(preWindTime);

        // Calculate start and target angles relative to initialLocalRot.
        // We'll swing from -half to +half (so it's centered).
        float half = swingAngle * 0.5f;
        float startZ = -half;
        float targetZ = half;

        // Apply start rotation
        float t = 0f;
        col.enabled = true; // Enable hitbox during swing (instant)
        // swing forward
        while (t < swingDuration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / swingDuration);
            float eased = swingCurve.Evaluate(normalized);
            float z = Mathf.Lerp(startZ, targetZ, eased);
            transform.localRotation = initialLocalRot * Quaternion.Euler(0, 0, z);
            yield return null;
        }

        // keep final pose briefly (optional)
        transform.localRotation = initialLocalRot * Quaternion.Euler(0, 0, targetZ);
        // small delay could be added here if wanted:
        // yield return new WaitForSeconds(0.02f);

        // recover back to initial
        t = 0f;
        while (t < recoverDuration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / recoverDuration);
            float eased = swingCurve.Evaluate(normalized);
            float z = Mathf.Lerp(targetZ, 0f, eased);
            transform.localRotation = initialLocalRot * Quaternion.Euler(0, 0, z);
            yield return null;
        }

        // finish
        transform.localRotation = initialLocalRot;
        col.enabled = startsEnabled; // restore collider state
        isSwinging = false;
    }

    // Simple hit detection
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isSwinging) return;
        // Example: damage enemy tagged "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // enemy �Ƀ_���[�W��^���鏈����������
            Debug.Log("Enemy hit: " + other.name);
            // var hp = other.GetComponent<EnemyHP>(); if (hp!=null) hp.TakeDamage(1);
        }
    }
}