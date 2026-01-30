using UnityEngine;

public class enemyHP : MonoBehaviour
{

    public int HP;
    public int cooltime;

    private bool inv;
    private int cool;
    private int fcool;
    private DamageEffect damageEffect;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inv=false;
        damageEffect = GetComponent<DamageEffect>(); 
    }

    // Update is called once per frame
    void Update()
    {
        

        if(inv)
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
                inv=false;
                cool=0;
                fcool=0;
            }
        }

        if(HP<=0){
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("wepon")&&!inv)
        {
            inv=true;
            //Debug.Log("Enemy hit: " + other.name);
            fcool=0;
            HP--;
        }
    }

    
}


