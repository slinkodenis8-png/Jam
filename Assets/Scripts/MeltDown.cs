using UnityEngine;

public class MeltDown : MonoBehaviour
{
   
    public float scaleX;
    public float scaleY;

    public bool Bonused;

    public float meltSpeed;

    float Xtarget;
    float Ytarget;

    public bool facingRight;
    private TrailRenderer trail;
    private void Start()
    {
        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;
        trail = GetComponent<TrailRenderer>();
    }
    private void Update()
    {
        transform.localScale = new Vector3(scaleX, scaleY, 0);
        trail.startWidth = scaleY;
        if(scaleX > 0.2f)
        {
        scaleX -= meltSpeed * Time.deltaTime;
        scaleY -= meltSpeed * Time.deltaTime;

        }
        else
        {
            Debug.Log("Вы померли!");
            Time.timeScale = 0;
        }

        if (Bonused)
        {
            scaleX = Mathf.Lerp(scaleX, Xtarget, meltSpeed * 0.1f);
            scaleY = Mathf.Lerp(scaleY, Ytarget, meltSpeed * 0.1f);
            if (Mathf.Round(scaleX * 100) == Mathf.Round(scaleY * 100))
            {
                Bonused = false;
            }
        }
    }

    public void Bonus()
    {
       
            Bonused = true;
            Xtarget = scaleX += 0.3f;
            Ytarget = scaleY += 0.3f;

        
    }
    
}
