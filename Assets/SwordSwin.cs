using System.Collections;
using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    [Header("�U���L�[")]
    public KeyCode attackKey = KeyCode.J;

    [Header("�U���ݒ�")]
    public float startAngle = 90f;     // �\���̊p�x�i��j
    public float endAngle = -45f;      // �U�艺�낷�p�x�i���j
    public float swingDuration = 0.15f; // �U�鑬��
    public float returnDuration = 0.1f; // ���ɖ߂�����

    private bool isSwinging = false;
    private Quaternion initialRotation;
    private Collider2D swordCollider;

    public GameObject targetObject;

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
            if (targetObject != null)
        {
            targetObject.SetActive(true);
           
        }
            StartCoroutine(SwingDownRoutine());
        
        }
    }

    IEnumerator SwingDownRoutine()
    {
        isSwinging = true;

        // �R���C�_�[ON
        if (swordCollider != null)
            swordCollider.enabled = true;

        float t = 0f;
        // �ォ�牺�֐U�艺�낷
        while (t < swingDuration)
        {
            t += Time.deltaTime;
            float ratio = Mathf.Clamp01(t / swingDuration);
            float zAngle = Mathf.Lerp(startAngle, endAngle, ratio);
            transform.localRotation = initialRotation * Quaternion.Euler(0, 0, zAngle);
            yield return null;
        }

        // �����Ԃ�u���i�����蔻��ێ��j
        yield return new WaitForSeconds(0.05f);

        // �R���C�_�[OFF
        if (swordCollider != null)
            swordCollider.enabled = false;

        // ���̊p�x�ɖ߂�
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
        targetObject.SetActive(false);
    }

    
}
