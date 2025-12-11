using UnityEngine;
using System.Collections; // コルーチンを使うために必要

public class DamageEffect : MonoBehaviour
{
    // 点滅させる対象のRendererコンポーネント
    // MeshRendererやSpriteRendererなど、表示に使っているものをInspectorから設定
    public Renderer targetRenderer; 

    // 点滅させる色（例: ダメージ時に赤くしたい）
    public Color damageColor = Color.red;

    // 元の色を保存しておく変数
    private Color originalColor;

    // 点滅時間（例: 0.5秒間点滅させる）
    public float duration = 0.5f;

    // 点滅の速さ（例: 0.1秒ごとに色を変える）
    public float flickerInterval = 0.1f;

    void Start()
    {
        // ゲーム開始時の色をオリジナルとして保存
        if (targetRenderer != null)
        {
            originalColor = targetRenderer.material.color;
        }
    }

    // ダメージを受けたときに外部から呼び出す関数
    public void FlashOnDamage()
    {
        // すでに点滅中なら、現在のコルーチンを止めて新しく開始する
        StopCoroutine(FlickerRoutine()); 
        StartCoroutine(FlickerRoutine());
    }

    // 点滅処理を行うコルーチン
    IEnumerator FlickerRoutine()
    {
        float startTime = Time.time;
        
        while (Time.time < startTime + duration)
        {
            // ダメージ色に変更
            targetRenderer.material.color = damageColor;
            // 指定した時間（例: 0.1秒）待つ
            yield return new WaitForSeconds(flickerInterval);

            // 元の色に戻す
            targetRenderer.material.color = originalColor;
            // 指定した時間（例: 0.1秒）待つ
            yield return new WaitForSeconds(flickerInterval);
            
            // whileループの条件に戻り、durationが過ぎていなければ続行
        }
        
        // 点滅が終わったら、必ず元の色に戻しておく
        targetRenderer.material.color = originalColor;
    }
}