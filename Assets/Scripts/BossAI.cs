using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossAI : MonoBehaviour
{
    public bool isAttack;
    public Transform[] movePoints;

    public int currentPoint;

    public float speed;

    public float attackTime;
    public float BreakTime;

    private Rigidbody2D rb;
    public bool ended;


    public Transform breakPoint;

    public Transform Player;

    public float offset;

    public BulletSettingsSO bulletData;
    public GameObject tracePrefab;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            if (ended)
            {
               
                StartCoroutine(att());
            }
    }
    public IEnumerator att()
    {
        ended = false;
        yield return new WaitForSeconds(1);
        if (isAttack)
        {
            ProjectileSpawner.Instance.FireFixedSpread(bulletData.bulletSettings, transform, Player);
        }
        else
        {

                TraceManager.Instance.DrawLineOverTime(transform.position, Player.position, tracePrefab, 0.9f, 0.3f, 0.3f);
            
            ProjectileSpawner.Instance.FireOnce(bulletData.bulletSettings, transform, Player);
        }
        ended = true;
    }

}
