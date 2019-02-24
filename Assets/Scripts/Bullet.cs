using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
	public float damage;
	public float speed;
	public Vector3 direction;

    void Start()
    {	
    	StartCoroutine("RotateBullet");
        Destroy(this.gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
    	if (Time.timeScale == 0) return;
     	this.GetComponent<Rigidbody>().velocity += direction * speed;
     	
    }

    public void Init(float damage, float speed, Vector3 direction/*, bool isExplosive*/) {
    	this.damage = damage;
    	this.speed = Mathf.Pow(speed, 2) * Time.deltaTime * 2f;
    	this.direction = direction;
    }

    void OnTriggerEnter(Collider other) {
    	Destroy(this.gameObject);
    	if (other.gameObject.CompareTag("Enemy")) {
    		//Destroy(this.gameObject);
    		other.gameObject.GetComponent<EnemyAI>().ReceiveDamage(this.damage);
    	}
    }
    IEnumerator RotateBullet() {
    	while (true) {
    		transform.Rotate(Random.Range(0, 80f), Random.Range(0, 80f), Random.Range(0, 80f));
    		yield return new WaitForSeconds(0.03f);
    	}
    }
}
