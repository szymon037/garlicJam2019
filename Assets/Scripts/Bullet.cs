using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
	public float damage;
	public float speed;
	public Vector3 direction;
	//public bool isExplosive = false;
    // Start is called before the first frame update
    void Start()
    {	
    	GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        Destroy(this.gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
     	transform.Translate(direction * speed);
    }

    public void Init(float damage, float speed, Vector3 direction/*, bool isExplosive*/) {
    	this.damage = damage;
    	this.speed = Mathf.Pow(speed, 2) * Time.deltaTime * 2f;
    	this.direction = direction;
    	//this.isExplosive = isExplosive;
    }

    void OnCollisionEnter(Collision other) {
    	Destroy(this.gameObject);
    	if (other.gameObject.CompareTag("Enemy")) {
    		//Destroy(this.gameObject);
    		other.gameObject.GetComponent<EnemyAI>().ReceiveDamage(this.damage);
    	}
    }
}
