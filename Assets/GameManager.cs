using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Prefab 参照")]
    
    public PlayerMove player; // プレイヤー

    private CsvMapLoader csvMapLoader;

    int enemyNum;

    private bool numflag;

    void Start()
    {
        GenerateMap();
        
        numflag=false;
        
    }

    void GenerateMap()
    {
        Vector2 position = Vector2.zero;
      
        // プレイヤー初期位置設定（地面の上）
        if (player != null)
        {
            //PlayerMove newplayer = Instantiate(player,new Vector3(10,10,0),Quaternion.identity);
            player.transform.position = new Vector2(10, 10);
        }
    }

    private void Update()
    {
        if(!numflag){
            csvMapLoader=GetComponent<CsvMapLoader>();
            enemyNum=csvMapLoader.enemyNum;
            Debug.Log("現在の敵 : " + enemyNum);
            numflag=true;
        }
        if(player.transform.position.y<=-10){
            SceneManager.LoadScene(2);
        }
        if(enemyNum<=0){
           SceneManager.LoadScene(3);
        }
    }

    public void EnemyDie(){
        enemyNum--;
        Debug.Log("現在の敵 : " + enemyNum);
    }
}
