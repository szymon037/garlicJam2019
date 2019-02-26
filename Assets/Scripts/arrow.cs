using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    public float speed = 5f;
    public float x = 0f;
    public float timer = 0f;
    public Vector3 startPos = Vector3.zero;
    public rotation rot; 

    void Start()
    {
        startPos = transform.position;
        GameObject player = GameObject.FindWithTag("Player"); 
        rot = player.GetComponent<rotation>();
    }

    // Update is called once per frame
    void Update()
    {        
        transform.position = new Vector3(transform.position.x, startPos.y + Mathf.Sin(x * speed), transform.position.z);
        x += Time.deltaTime;

        if (rot.destroyArrow)
        {
            timer += Time.deltaTime;
            transform.localScale *= 1 / Mathf.Pow(2, timer / 4);
        }
        
        if (transform.localScale.x <= 10)
            Destroy(this.gameObject);

    }
}
