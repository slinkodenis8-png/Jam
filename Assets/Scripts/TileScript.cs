using UnityEngine;
public class TileScript : MonoBehaviour
{
    private Transform endPos;
    private Transform startPos;
    private TileMoving mov;
    [SerializeField] private GameObject inst;
    private void Start()
    {
        endPos = GameObject.Find("endpos").transform;
        startPos = GameObject.Find("startpos").transform;
        mov = FindAnyObjectByType<TileMoving>();
    }
    private void FixedUpdate()
    {
        if(Mathf.Round(gameObject.transform.position.y) == Mathf.Round(endPos.transform.position.y))
        {
           GameObject obj =  Instantiate(inst, startPos.position,Quaternion.identity);
            obj.transform.SetParent(GameObject.Find("Grid").transform);
            Destroy(gameObject);
        }
        transform.Translate(transform.up * mov.plusSpeed * Time.fixedDeltaTime);
    }
}
