using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
	public float health;
  	public float damage;
  	public string enemyName;
  	public float speed;
  	public float knockbackOnDamageTaken;
  	public float knockbackPower;
  	public int currencyOnKill;
  	public float runAwayTimer = 0f;
  	public static Transform player = null;

  	void OnDestroy() {
  		/*add animation or sth*/
  	}

    // Start is called before the first frame update
    void Start()
    {
        if (player == null) player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {	
        if (runAwayTimer > 0f) {
        	runAwayTimer -= Time.deltaTime;
        	RunAwayFromPlayer();
        } else {
        	ChasePlayer();
        }
    }

    public void ChasePlayer() {
    	this.transform.position = Vector3.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
    }

    public void BeginRunAway(float timerValue) {
    	this.runAwayTimer = timerValue;
    }

    public void RunAwayFromPlayer() {
    	this.transform.position = Vector3.MoveTowards(this.transform.position, player.position, -speed * Time.deltaTime);
    }

    public void ReceiveDamage(float val) {
    	this.health -= val;
    	if (this.health <= 0f) Die();
    	this.gameObject.GetComponent<Rigidbody>().AddForce(-transform.forward * knockbackOnDamageTaken, ForceMode.Impulse);
    }

    public void Die() {
    	Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision other) {
    	if (other.gameObject.CompareTag("Player") && !PlayerStats.GetInstance().flags["isHit"]) {
    		PlayerStats.GetInstance().ToggleFlag("isHit", true);
    		PlayerStats.GetInstance().ChangeHealth(-this.damage);
    	}
    } 
}
