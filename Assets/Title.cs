using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections; // コルーチンを使うために必要


public class Title : MonoBehaviour
{

    private bool canMove = false; // 操作可能かどうかのフラグ
    [SerializeField] private float waitTime = 2.0f; // 操作不能にする時間（秒）

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // シーン開始時にコルーチンを呼び出す
        StartCoroutine(DisableControlRoutine());
    }

    IEnumerator DisableControlRoutine()
    {
        canMove = false; // 操作不能にする
        yield return new WaitForSeconds(waitTime); // 指定した秒数待機
        canMove = true; // 操作可能にする
    }

    // Update is called once per frame
    void Update()
    {
         // canMoveがfalseの時は、これ以降の処理（移動入力など）を読み飛ばす
        if (!canMove) return;

        if(Keyboard.current.spaceKey.isPressed){
            SceneManager.LoadScene(1);
        }
    }
}
