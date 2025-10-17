using System.Collections;
using UnityEngine;

public class swordMove : MonoBehaviour
{
    public KeyCode attackKey = KeyCode.J;
    public float swingAngle = 90f;
    public float swingDuration = 0.2f;

    private bool isSwinging = false;
    private Quaternion initialRot;
    private Collider2D swordCollider;

    void Start()
    {
        // Žq‚Ì SwordMove ‚É‚ ‚é Collider2D ‚ðŽæ“¾
        swordCollider = GetComponentInChildren<Collider2D>();
        if (swordCollider != null)
            swordCollider.enabled = false;

        initialRot = transform.localRotation;
    }

    void Update()
    {
        if (!isSwinging && Input.GetKeyDown(attackKey))
        {
            StartCoroutine(SwingRoutine());
        }
    }

    IEnumerator SwingRoutine()
    {
        isSwinging = true;
        if (swordCollider != null)
            swordCollider.enabled = true;

        float t = 0;
        while (t < swingDuration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / swingDuration);
            float angle = Mathf.Lerp(-swingAngle / 2f, swingAngle / 2f, normalized);
            transform.localRotation = initialRot * Quaternion.Euler(0, 0, angle);
            yield return null;
        }

        transform.localRotation = initialRot;
        if (swordCollider != null)
            swordCollider.enabled = false;
        isSwinging = false;
    }
}
