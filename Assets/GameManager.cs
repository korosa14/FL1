using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    [Header("Tilemap 設定")]
    public Tilemap groundTilemap;
    public TileBase groundTile;

    [Header("Prefab 設定")]
    public GameObject enemyPrefab;
    public GameObject itemPrefab;
    public GameObject playerPrefab;

    [Header("マップ　設定")]
    public string csvFileName = "map";
    public bool loadFromCSV = true;

    private int[,] samplrMap = new int[,]
    {
        {0,0,0,0,0,0,0},
        {0,1,1,0,0,1,0},
        {0,1,0,0,1,1,0},
        {1,1,1,0,0,0,0}
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeGame();
    }

    //ゲーム開始時の初期化

    void InitializeGame()
    {
        int[,] mapData = loadFromCSV ? LoadCSV(csvFileName) : samplrMap;

        //マップ生成
        GenerateMap(mapData);

        //プレイヤー生成
        if(playerPrefab !=null)
        {
            Vector3 spawnPos = new Vector3(1, 1, 0);
            Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        }
    }

    //CSVまたは配列からマップ生成
    void GenerateMap(int[,]map)
    {
        groundTilemap.ClearAllTiles();

        int height = map.GetLength(0);
        int width = map.GetLength(1);

        for(int y=0; y < height; y++)
        {
            for(int x=0;x<width;x++)
            {
                int code = map[y, x];
                Vector3Int tilePos = new Vector3Int(x, -y, 0);

                switch(code)
                {
                    case 1://地面
                        groundTilemap.SetTile(tilePos, groundTile);
                        break;

                    case 2://敵
                        if (enemyPrefab)
                            Instantiate(enemyPrefab, groundTilemap.CellToWorld(tilePos) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                        break;

                    case 3://アイテム
                        if (itemPrefab)
                            Instantiate(itemPrefab, groundTilemap.CellToWorld(tilePos) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                        break;
                }
            }
        }

        groundTilemap.RefreshAllTiles();
    }

    //CSVファイル読み込み
    int[,] LoadCSV(string fileName)
    {
        TextAsset csvData = Resources.Load<TextAsset>(fileName);
        if (csvData==null)
        {
            Debug.LogWarning("CSVが見つかりません:" + fileName + "csv");
            return samplrMap;
        }

        string[] lines = csvData.text.Trim().Split('\n');
        int height = lines.Length;
        int width = lines[0].Split(',').Length;

        int[,] map = new int[height, width];

        for(int y=0;y<height;y++)
        {
            string[] cols = lines[y].Trim().Split(',');
            for(int x=0;x<width;x++)
            {
                int.TryParse(cols[x], out map[y, x]);
            }
        }

        return map;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
