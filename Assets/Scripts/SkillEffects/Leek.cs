using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum LeekType : byte {
	Spring,
	Summer,
	Fall,
	Winter
}
[System.Serializable]
public struct LeekStats {
	public float health;
	public float fireRate;
	public float speed;
	public float aggroRadius;
	public float decayTime;
	public float damage;

	public LeekStats(float _health, float _fireRate, float _speed, float aggro, float _decay, float damage) {
		health = _health;
		fireRate = _fireRate;
		speed = _speed;
		aggroRadius = aggro;
		decayTime = _decay;
		this.damage=  damage;
	}
}

public enum EnemyState  {
	Searching = 2,
	Attacking
}

[RequireComponent(typeof(ParticleSystem))]
public class Leek : MonoBehaviour
{
	public static Dictionary<LeekType, LeekStats> leekTypeStats = new Dictionary<LeekType, LeekStats>() {
		{LeekType.Spring, new LeekStats(100f, 0.35f, 5f, 100f, 20f, 15f)},
		{LeekType.Summer, new LeekStats(133f, 0.28f, 6.5f, 100f, 25f, 18f)},
		{LeekType.Fall, new LeekStats(150f, 0.21f, 8f, 100f, 30f, 21f)},
		{LeekType.Winter, new LeekStats(166f, 0.14f, 10f, 100f, 35f, 24f)}
	};

	public LeekStats stats = new LeekStats();
	public ParticleSystem ps;
	private EnemyState state;
	public GameObject bulletPrefab;
	public Transform target;
	public Transform oldTarget = null;
	public Transform shootpoint;
	public float changeDirectionTimer = 0f;
	public float shotTimer = 0f;

	void Start()
	{
		ps = gameObject.GetComponent<ParticleSystem>();
		ps.Play();
		state = EnemyState.Searching;
	}

	void Update() {
		if (state == EnemyState.Searching) {
			SeekEnemies();
			if (changeDirectionTimer > 0f) {
				changeDirectionTimer -= Time.deltaTime;
			} else {
				//transform.rotation = Quaternion.Lerp();
				changeDirectionTimer = Random.Range(2f, 6f);
				transform.Rotate(0f, Random.Range(40f, 75f), 0f);
			}
			transform.Translate(-transform.forward * stats.speed * Time.deltaTime);
		} else {
			Chase();
		}
		Debug.Log(((int)this.state).ToString());
		oldTarget = target;
		if (shotTimer > 0f) shotTimer -= Time.deltaTime;
		if (target == null && state == EnemyState.Attacking) ToggleState();
	}
    

    public void Init(LeekType type) {
    	this.stats = leekTypeStats[type];
    	Destroy(this.gameObject, this.stats.decayTime);
    }

    public void SeekEnemies() {
    	Collider[] enemies = (Physics.OverlapSphere(this.transform.position, stats.aggroRadius) as IEnumerable<Collider>).Where(x => x.gameObject.CompareTag("Enemy")).ToArray();
    	if (enemies.Length <= 0) return;
    	target = FindClosestTarget(enemies);
    	ToggleState();
    }

    public void RandomWalker() {

    }

    public Transform FindClosestTarget(Collider[] objects) {
    	if (objects.Length <= 0) return null;
    	if (objects.Length == 1) return objects[0].gameObject.transform;
    	float mindist = Vector3.Distance(objects[0].gameObject.transform.position, this.transform.position);
    	Transform _target = objects[0].gameObject.transform;
    	foreach (var obj in objects) {
    		float temp = Vector3.Distance(obj.gameObject.transform.position, this.transform.position);
    		if (temp < mindist) {
    			mindist = temp;
    			_target = obj.gameObject.transform;
    		}
    	}
    	return _target;
    } 

    public void Chase() {
    	try {
    		this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, stats.speed * Time.deltaTime);
    	} catch (System.Exception) {}
    }

    public void ToggleState() {
    	if (state == EnemyState.Searching) {
    		state = EnemyState.Attacking;
    	} else {
    		state = EnemyState.Searching;
    	}
    }

    void OnCollisionEnter(Collision other) {
    	if (other.gameObject.CompareTag("Enemy") && shotTimer <= 0f) {
    		shotTimer = this.stats.fireRate;
    		other.gameObject.GetComponent<EnemyAI>().ReceiveDamage(this.stats.damage);
    	}
    }

    //public void 
}
