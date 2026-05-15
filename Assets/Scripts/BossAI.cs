using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossAI : MonoBehaviour
{
    public bool isAttack;
    public Transform[] movePoints;

    public int currentPoint;

    public float speed;
    [Range (0,100)]
    public int health=100;

    public float attackTime;
    public float BreakTime;

    private Rigidbody2D rb;
    public bool ended;
    public bool scndStage;
    public bool used;


    public Transform breakPoint;

    public Transform Player;

    public float offset;

    public BulletSettingsSO bulletData;
    public GameObject tracePrefab;

    public Transform[] higherPoints;
    public Transform[] lowerPoints;

    private Animator anim;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


 

    

private void FixedUpdate()
    {
        if (health <=30)
        {
            isAttack = false;
            scndStage = true;
        }
        if (isAttack && !scndStage)
        {
            anim.SetBool("isAttack", true);
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
            if(!scndStage)
            {

            anim.SetBool("isAttack", false);
            }
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
        if (scndStage && used == false)
        {
            StartCoroutine(scndAtt());
        }
    }
    public IEnumerator att()
    {
        ended = false;
        yield return new WaitForSeconds(1);
        if (isAttack && !scndStage)
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

    public IEnumerator scndAtt()
    {
        used = true;
        bool queue = false;
        while (true)
        {
            
            queue = !queue;
        if (queue)
        {
            for(int i = 0; i < higherPoints.Length; i++)
            {
                TraceManager.Instance.DrawLineOverTime(higherPoints[i].transform.position, lowerPoints[i].position, tracePrefab, 0.9f, 0.3f, 0.3f);

                ProjectileSpawner.Instance.FireOnce(bulletData.bulletSettings, higherPoints[i].transform, lowerPoints[i].transform);
            }
        }
        else
        {
            for (int i = 0; i < higherPoints.Length; i++)
            {
                TraceManager.Instance.DrawLineOverTime(lowerPoints[i].transform.position, higherPoints[i].position, tracePrefab, 0.9f, 0.3f, 0.3f);

                ProjectileSpawner.Instance.FireOnce(bulletData.bulletSettings, lowerPoints[i].transform, higherPoints[i].transform);
            }
        }
            yield return new WaitForSeconds(3);
        }
    }

}
