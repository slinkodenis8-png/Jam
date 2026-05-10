using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UI;

public class WOFAI : MonoBehaviour, IDamageable
{
    public int currentPhase = 0;
    public Image slider;
    public GameObject winPanel;
    public int maxHealth = 400;
    [Range(0, 400)]
    public int health;
    public PhaseData currentPhaseData;

    public Transform player;

    public Transform[] shotPoints1;
    public Transform[] shotPoints2;
    public Transform[] shotPoints3;

    [Header("Movement")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Phase Settings")]
    [SerializeField] private List<PhaseData> phases = new List<PhaseData>();

    void Start()
    {
        Pooler.ClearAllPools();

        if (rb == null) rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        currentPhaseData = phases.Count > 0 ? phases[0] : new PhaseData();
        health = maxHealth;
        StartAttack();
    }

    void StartAttack()
    {
        StopAllCoroutines();
        StartCoroutine(ForcePhase(phases[0]));
    }


    void FixedUpdate()
    {
        MoveVertically();
    }

    void MoveVertically()
    {
        Vector2 movement = Vector2.down * currentPhaseData.moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    public void TakeDamage(int amount)
    {
        health = Mathf.Max(0, health - amount);
        slider.fillAmount = (float)health / maxHealth;
        if(health <= 0)
        {
            Time.timeScale = 0;
            winPanel.SetActive(true);
        }
        CheckPhase();
    }

    void CheckPhase()
    {
        float healthRatio = (float)health / maxHealth;

        if (currentPhase < phases.Count - 1)
        {
            float nextPhaseThreshold = phases[currentPhase + 1].healthThreshold;
            if (healthRatio <= nextPhaseThreshold)
            {
                StartCoroutine(NextPhase());
            }
        }
    }

    IEnumerator NextPhase()
    {
        yield return new WaitForSeconds(2f);
        StopAllCoroutines();
        currentPhase++;
        if (currentPhase < phases.Count)
        {
            currentPhaseData = phases[currentPhase];
            WOFAction randomAction = currentPhaseData.actions[UnityEngine.Random.Range(0, currentPhaseData.actions.Length)];
            if (randomAction.targetPlayer)
            {
                StartCoroutine(ExecuteAction(randomAction, player));
            }
            else
            {
                StartCoroutine(ExecuteAction(randomAction));
            }

        }
    }

    IEnumerator ForcePhase(PhaseData phase)
    {
        yield return new WaitForSeconds(2f);
        currentPhaseData = phase;
        WOFAction randomAction = currentPhaseData.actions[UnityEngine.Random.Range(0, currentPhaseData.actions.Length)];
        if (randomAction.targetPlayer)
        {
            StartCoroutine(ExecuteAction(randomAction, player));
        }
        else
        {
            StartCoroutine(ExecuteAction(randomAction));
        }
    }

    void NextAction()
    {
        if (currentPhaseData.actions.Length == 0) return;

        WOFAction randomAction = currentPhaseData.actions[UnityEngine.Random.Range(0, currentPhaseData.actions.Length)];
        if (randomAction.targetPlayer)
        {
            StartCoroutine(ExecuteAction(randomAction, player, randomAction.shotDelayAfterTrace));
        }
        else
        {
            StartCoroutine(ExecuteAction(randomAction, null, randomAction.shotDelayAfterTrace));
        }
    }

    public IEnumerator ExecuteAction(WOFAction action, Transform target = null, float shotDelayAfterTrace = 0)
    {
        if (action.projectileData == null)
        {
            NextAction();
            yield break;
        }

        for (int i = 0; i < action.amountOfShots; i++)
        {
            foreach (Transform pos in action.FirePoints)
            {
                if (target != null)
                {
                    if (shotDelayAfterTrace >= 0)
                    {
                        TraceManager.Instance.DrawLineOverTime(pos.position, target.position, action.tracePrefab);
                    }
                }
                else
                {
                    if (shotDelayAfterTrace >= 0)
                    {
                        Vector3 target_ = pos.position + (Vector3)(Vector2.down * 15f);
                        TraceManager.Instance.DrawLineOverTime(pos.position, target_, action.tracePrefab);
                    }
                }
            }

            StartCoroutine(DelayedShot(action, target, shotDelayAfterTrace));

            yield return new WaitForSeconds(action.DelayBetweenShots);
        }

        yield return new WaitForSeconds(action.actionDuration - action.DelayBetweenShots * action.amountOfShots);
        NextAction();
    }

    IEnumerator DelayedShot(WOFAction action, Transform target = null, float shotDelayAfterTrace = 0)
    {
        if (shotDelayAfterTrace >= 0)
        {
            yield return new WaitForSeconds(shotDelayAfterTrace);
        }

        foreach (Transform pos in action.FirePoints)
        {
            if (target != null)
            {
                ProjectileSpawner.Instance.FireOnce(action.projectileData.bulletSettings, pos, target);
            }
            else
            {
                Vector3 target_ = pos.position + (Vector3)(Vector2.down * 15f);
                ProjectileSpawner.Instance.FireOnce(action.projectileData.bulletSettings, pos.position, target_);
            }
        }
        yield return null;
    }
}



[Serializable]
public struct PhaseData
{
    [Range(0, 1)]
    public float healthThreshold;
    public float moveSpeed;
    public WOFAction[] actions;
}

[Serializable]
public struct WOFAction
{
    public bool targetPlayer;
    public float actionDuration;
    public int amountOfShots;
    public float DelayBetweenShots;
    public BulletSettingsSO projectileData;
    public Transform[] FirePoints;
    public GameObject tracePrefab;
    public float shotDelayAfterTrace;
}


