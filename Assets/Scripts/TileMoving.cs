using UnityEngine;

public class TileMoving : MonoBehaviour
{
    [SerializeField] private float tileSpeed;
    [SerializeField] private float changeSpeed;

    private bool needToChange;
    private float needSpeed;

    private void Start()
    {
        needSpeed = tileSpeed;
        Physics2D.gravity = new Vector2(0, 9.81f);
        GameObject.Find("Player").GetComponent<Rigidbody2D>().gravityScale = tileSpeed * 5;
    }
    public float plusSpeed
    {
        get=>tileSpeed;
        set
        {
            needToChange=true;
            needSpeed += value;
            
        }
    }
    private void Update()
    {
        if (needToChange)
        {
            tileSpeed = Mathf.MoveTowards(tileSpeed, needSpeed, changeSpeed*Time.deltaTime);
            GameObject.Find("Player").GetComponent<Rigidbody2D>().gravityScale = Mathf.MoveTowards(GameObject.Find("Player").GetComponent<Rigidbody2D>().gravityScale, GameObject.Find("Player").GetComponent<Rigidbody2D>().gravityScale+ needSpeed, changeSpeed * Time.deltaTime);
            if (tileSpeed > needSpeed)
            {
                needToChange = false;
            }
        }
    }
   
}
