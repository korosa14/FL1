using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class CsvMapLoader : MonoBehaviour
{
    public Tilemap targetTilemap; // タイルを配置するTilemap
    public Tile[] tilePalette;   // CSVの数値に対応するタイルの配列
    public string csvFileName = ""; // Resourcesフォルダ内のCSVファイル名

    public GameObject enemy1;

    void Start()
    {
        LoadMapFromCsv();
    }

    void LoadMapFromCsv()
    {
        // ResourcesフォルダからCSVファイルをTextAssetとして読み込む
        TextAsset csvFile = Resources.Load<TextAsset>(csvFileName);

        if (csvFile == null)
        {
            Debug.LogError("CSVファイルが見つかりませんでした: " + csvFileName);
            return;
        }

        string[] lines = csvFile.text.Split('\n');

        // 縦方向のループ（行）
        for (int y = 0; y < lines.Length; y++)
        {
            string line = lines[y];
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] values = line.Split(',');

            // 横方向のループ（列）
            for (int x = 0; x < values.Length; x++)
            {
                if (int.TryParse(values[x].Trim(), out int tileId))
                {
                    // tileIdがtilePaletteの範囲内にあるか確認
                    if (tileId == 1 && tileId < tilePalette.Length)
                    {
                        Tile tileToPlace = tilePalette[tileId];
                        // 座標を計算し、Tilemapにタイルをセット
                        // CSVの並びとUnityの座標系を合わせるためy座標を反転させる
                        targetTilemap.SetTile(new Vector3Int(x, lines.Length - 1 - y, 0), tileToPlace);
                    }else if (tileId == 2)
                    {
                        GameObject newObject = Instantiate(enemy1, new Vector3(x, lines.Length - 1 - y, 0), Quaternion.identity);
                    }
                }
            }
        }
        Debug.Log("CSVからのマップロードが完了しました。");
    }
}