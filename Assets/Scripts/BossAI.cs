using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public bool isAttack;
    public GameObject bullet;
    public GameObject bulletPack;
    public Transform[] movePoints;

    public int currentPoint;

    public float speed;

    public float attackTime;
    public float BreakTime;

    private Rigidbody2D rb;
    public bool ended;

    private int rand;

    public Transform breakPoint;

    public Transform Player;

    public float offset;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


 

    void Update()
    {
        if (Player != null)
        {
            Vector3 difference = Player.position - transform.position;
            difference.Normalize();
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
        }
    }

private void FixedUpdate()
    {
        if (isAttack)
        {
            BreakTime = 4;
            transform.position = Vector3.MoveTowards(transform.position, movePoints[currentPoint].position, speed * Time.deltaTime);
            if (Mathf.Round(transform.position.x) == Mathf.Round(movePoints[currentPoint].position.x) && Mathf.Round(transform.position.y) == Mathf.Round(movePoints[currentPoint].position.y))
            {
                if (currentPoint != movePoints.Length - 1)
                {
                    currentPoint++;
                }
                else
                {
                    currentPoint = 0;
                }
            }
            if (ended)
            {
                rand = Random.Range(0, 3);
                StartCoroutine(att());
            }
            attackTime -= Time.deltaTime;
            if(attackTime < 0)
            {
                isAttack = false;
            }
        }
        else
        {
            attackTime = 6;
           transform.position =  Vector3.MoveTowards(transform.position, breakPoint.position, speed * Time.deltaTime);
            BreakTime -= Time.deltaTime;
            if(BreakTime < 0)
            {
                isAttack = true;
            }
        }
    }
    public IEnumerator att()
    {
        ended = false;
        yield return new WaitForSeconds(2);
        if (rand == 0)
        {
            Instantiate(bullet,transform.position,transform.rotation);
        }
        else
        {
           
            Instantiate(bulletPack,transform.position, transform.rotation);
        }
        ended = true;
    }

}
