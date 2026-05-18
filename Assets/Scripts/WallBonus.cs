using System.Collections;
using UnityEngine;

public class WallBonus : MonoBehaviour
{
    public GameObject bon;
    private bool instant;
    [SerializeField ]private bool move;
    private float wait = 10;
    private void Start()
    {
        StartCoroutine(timer());
      
    }
    private void FixedUpdate()
    {
        if(wait<0)  GetComponent<Rigidbody2D>().gravityScale = GameObject.Find("Player").GetComponent<Rigidbody2D>().gravityScale / 20;
        wait-=Time.fixedDeltaTime;
    }
    IEnumerator timer()
    {
        yield return new WaitForSeconds(15);
        inst();
        Destroy(gameObject);
    }
    public void inst()
    {
        if (!instant)
        {

        Instantiate(bon, new Vector2(Random.Range(-10,10), -10), Quaternion.identity);
        instant = true;
        }
    }
   
}
