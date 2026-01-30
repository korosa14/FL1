using UnityEngine;

public class srimmove : MonoBehaviour
{
    
    private enemymove _Enemymove;
    private enemyHP _EnemyHP;

    private float direction;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _Enemymove = GetComponent<enemymove>();
        _EnemyHP= GetComponent<enemyHP>();
        if(_Enemymove != null)
        {
            direction = _Enemymove.GetDirection();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
