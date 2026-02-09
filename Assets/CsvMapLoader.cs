using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class CsvMapLoader : MonoBehaviour
{
    public Tilemap targetTilemap; // �^�C����z�u����Tilemap
    public Tile[] tilePalette;   // CSV�̐��l�ɑΉ�����^�C���̔z��
    public string csvFileName = ""; // Resources�t�H���_����CSV�t�@�C����

    public GameObject enemy1;

    public int enemyNum;

    void Start()
    {
        LoadMapFromCsv();
    }

    void LoadMapFromCsv()
    {
        // Resources�t�H���_����CSV�t�@�C����TextAsset�Ƃ��ēǂݍ���
        TextAsset csvFile = Resources.Load<TextAsset>(csvFileName);

        if (csvFile == null)
        {
            Debug.LogError("CSV�t�@�C����������܂���ł���: " + csvFileName);
            return;
        }

        string[] lines = csvFile.text.Split('\n');

        // �c�����̃��[�v�i�s�j
        for (int y = 0; y < lines.Length; y++)
        {
            string line = lines[y];
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] values = line.Split(',');

            // �������̃��[�v�i��j
            for (int x = 0; x < values.Length; x++)
            {
                if (int.TryParse(values[x].Trim(), out int tileId))
                {
                    // tileId��tilePalette�͈͓̔��ɂ��邩�m�F
                    if (tileId == 1 && tileId < tilePalette.Length)
                    {
                        Tile tileToPlace = tilePalette[tileId];
                        // ���W���v�Z���ATilemap�Ƀ^�C�����Z�b�g
                        // CSV�̕��т�Unity�̍��W�n�����킹�邽��y���W�𔽓]������
                        targetTilemap.SetTile(new Vector3Int(x, lines.Length - 1 - y, 0), tileToPlace);
                    }else if (tileId == 2)
                    {
                        GameObject newObject = Instantiate(enemy1, new Vector3(x, lines.Length - 1 - y, 0), Quaternion.identity);
                        enemyNum++;
                    }
                }
            }
        }
        Debug.Log("CSV����̃}�b�v���[�h���������܂����B");
    }
}