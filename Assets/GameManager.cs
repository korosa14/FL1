using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefab 参照")]
    
    public PlayerMove player; // プレイヤー

    

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        Vector2 position = Vector2.zero;
      
        // プレイヤー初期位置設定（地面の上）
        if (player != null)
        {
            player.transform.position = new Vector2(10, 10);
        }
    }

    private void Update()
    {
        
    }
}
