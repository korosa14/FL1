using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefab 参照")]
    public GameObject block;  // 通常ブロック
    public GameObject player; // プレイヤー

    // 2Dマップ（1=地面、0=空間）
    int[,] map =
    {
        {1,1,1,1,1,1,1,1,1,1},
        {1,0,0,0,0,0,0,0,0,1},
        {1,0,0,0,0,0,0,0,0,1},
        {1,0,0,0,0,0,0,0,0,1},
        {1,0,0,0,0,0,0,0,0,1},
        {1,1,1,1,1,1,1,1,1,1}
    };

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        Vector2 position = Vector2.zero;
        int height = map.GetLength(0);
        int width = map.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                position.x = x;
                position.y = -y; // 下方向に伸びるように

                if (map[y, x] == 1)
                {
                    Instantiate(block, position, Quaternion.identity);
                }
            }
        }

        // プレイヤー初期位置設定（地面の上）
        if (player != null)
        {
            player.transform.position = new Vector2(2, -2);
        }
    }
}
