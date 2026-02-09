using UnityEngine;

public class enemyHP : MonoBehaviour
{

    public int HP;
    public int cooltime;

    private int cool;
    private int fcool;
    private DamageEffect damageEffect;

    private enemymove enemyMove;
    
    private GameManager gameManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damageEffect = GetComponent<DamageEffect>(); 
        enemyMove=GetComponent<enemymove>();
        gameManager=GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        

        if(enemyMove.GetInv())
        {
            damageEffect.FlashOnDamage();
            fcool++;
            if(fcool>=60)
            {
                cool++;
                fcool=0;
            }
            if(cool>=cooltime)
            {
                enemyMove.SetInv(false);
                cool=0;
                fcool=0;
            }
        }

        if(HP<=0 || enemyMove.Getfall()){
            gameManager.EnemyDie();
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("wepon")&&!enemyMove.GetInv())
        {
            fcool=0;
            HP--;
            enemyMove.SetInv(true);
            enemyMove.PlayKnockback(10f);
        }
    }

    
}


