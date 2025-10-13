using UnityEngine;
using System.IO;

public class CsvReader : MonoBehaviour
{
    // CSVファイルをアタッチするための変数
    public TextAsset csvFile;
    public const int wide=10;
    public const int high=10;
    int[,] map = new int [high,wide];
    void Start()
    {
        if (csvFile != null)
        {
            // CSVファイルを文字列として取得
            string csvText = csvFile.text;
            
            // 行ごとに分割
            string[] lines = csvText.Split('\n');

            int xLine=0;
            int yLine=0;

            foreach (string line in lines)
            {
                // カンマで列を分割
                string[] cells = line.Split(',');
                
                // データを利用
                foreach (string cell in cells)
                {
                    Debug.Log(cell.Trim()); // 前後の空白を削除して表示
                    map[yLine,xLine]=int.Parse(cell);
                    xLine++;
                }
                xLine=0;
                yLine++;
            }
        }
    }

}