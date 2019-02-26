using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	public float health;
  	public float damage;
  	public string enemyName;
  	public float speed;
  	public float knockbackOnDamageTaken;
  	public float knockbackPower;
  	public int currencyOnKill;
  	public int lower_bound;
  	public int upper_bound;
  	public int scoreValue;
  	public float runAwayTimer = 0f;
  	public static Transform player = null;
  	public CurrentBiome biome = CurrentBiome.Forest;
  	public float gravityAngle;
  	public Rigidbody rb;
  	public float gravity = 10f;
    public float angleZ = 0f;

    public bool rightLeft = false;
    public bool topBottom = false;

    public Vector3 lookPos = Vector3.zero;

       
    void OnDestroy() {
  		/*add animation or sth*/
  	}

    // Start is called before the first frame update
    public void Start()
    {
    	this.currencyOnKill = Random.Range(lower_bound, upper_bound);
        if (player == null) player = GameObject.FindWithTag("Player").transform;
        health = Random.Range(120f, 180f);
        damage = Random.Range(15f, 30f);
        speed = Random.Range(3f, 6f);
        scoreValue = Random.Range(100, 201);
        Destroy(this.gameObject, 60f);
        rb = GetComponent<Rigidbody>();
        //transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        lookPos = transform.position - player.position;

        if (runAwayTimer > 0f) {
        	runAwayTimer -= Time.deltaTime;
        	RunAwayFromPlayer();
        } else if (Vector3.Distance(player.position, transform.position) < 40f){
        	try{
                if (topBottom)
        		    transform.LookAt(player);
                else if (rightLeft)
                    transform.rotation = Quaternion.Euler(90f + Mathf.Atan2(lookPos.z, lookPos.y) * Mathf.Rad2Deg, 0f, angleZ);

                ChasePlayer();
        	} catch (System.Exception) {}
        }
        if (topBottom)
            rb.velocity = (angleZ == 0f) ? -transform.up * gravity : transform.up * gravity;
        else if (rightLeft)
            rb.velocity = /*(angleZ == 0f) ?*/ -transform.right * gravity /*: transform.right * gravity*/;


        //transform.rotation = Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, 0f));
        // transform.rotation = Quaternion.Euler(-90f, transform.eulerAngles.y + 110f, transform.eulerAngles.z - gravityAngle);
        if (topBottom)
             transform.rotation = Quaternion.Euler(new Vector3(0f, transform.rotation.eulerAngles.y, angleZ));
        /*else if (rightLeft)
             transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, 0f, angleZ));*/
        //transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, 0f, 0f)); 
        if (Vector3.Distance(player.position, transform.position) > 175f)
            Die();


    }

    public void ChasePlayer() {
    	this.transform.position = Vector3.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
    	//this.transform.position = new Vector3(this.transform.position.x, 0f, this.transform.position.z);
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
    	//this.gameObject.GetComponent<Rigidbody>().AddForce(-transform.forward * knockbackOnDamageTaken, ForceMode.Impulse);
    }

    public void Die() {
    	GameObject leek = GameObject.FindWithTag("Leek");
		if (leek != null) {
			leek.GetComponent<Leek>().target = null;
			leek.GetComponent<Leek>().ToggleState();
		}
		AddCurrencyToPlayer();
		EnemySpawner.enemies--;
    	Destroy(this.gameObject);
    }

    public void AddCurrencyToPlayer() {
    	PlayerStats.GetInstance().AcquireCurrency(this.currencyOnKill);
    	UpgradeManager.instance.UpdateCurrency();
    }

    void OnCollisionEnter(Collision other) {
    	if (other.gameObject.CompareTag("Player") && !PlayerStats.GetInstance().flags["isHit"]) {
    		PlayerStats.GetInstance().ToggleFlag("isHit", true);
    		PlayerStats.GetInstance().ChangeHealth(-this.damage);
    	}
    } 

   public void AddScore() {
   		PlayerStats.GetInstance().score += this.scoreValue;
   }
   public void SetAngle(float angle) {
        angleZ = angle;
   }
}
