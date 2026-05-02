using System.Collections;
using UnityEngine;

public class BonusSpawn : MonoBehaviour
{
    public Transform[] points;
    public GameObject prefab;
    public float timing;
    private void Start()
    {
        StartCoroutine(ie());
    }
    public IEnumerator ie()
    {
        while (true)
        {
            yield return new WaitForSeconds(timing);
            Transform pos = points[Random.Range(0, points.Length)];
            if (pos.childCount != 0)
            {
                while (pos.childCount > 0)
                {
                    pos = points[Random.Range(0, points.Length)];
                }
            }
                Transform Inst = Instantiate(prefab.transform, pos.position, Quaternion.identity);
                Inst.SetParent(pos);
            
        }
    }
}
