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

	public LeekStats(float _health, float _fireRate, float _speed, float aggro, float _decay) {
		health = _health;
		fireRate = _fireRate;
		speed = _speed;
		aggroRadius = aggro;
		decayTime = _decay;
	}
}

public enum EnemyState : byte {
	Searching,
	Attacking
}

[RequireComponent(typeof(ParticleSystem))]
public class Leek : MonoBehaviour
{
	public static Dictionary<LeekType, LeekStats> leekTypeStats = new Dictionary<LeekType, LeekStats>() {
		{LeekType.Spring, new LeekStats(100f, 0.35f, 5f, 5f, 20f)},
		{LeekType.Summer, new LeekStats(133f, 0.28f, 6.5f, 6f, 25f)},
		{LeekType.Fall, new LeekStats(150f, 0.21f, 8f, 8f, 30f)},
		{LeekType.Winter, new LeekStats(166f, 0.14f, 10f, 10f, 35f)}
	};

	public LeekStats stats = new LeekStats();
	public ParticleSystem ps;
	private EnemyState state;
	public GameObject bulletPrefab;
	public Transform target;

	void Start()
	{
		ps = gameObject.GetComponent<ParticleSystem>();
		ps.Play();
		state = EnemyState.Searching;
	}

	void Update() {
		if (state == EnemyState.Searching) {
			SeekEnemies();
		} else {
			Chase();
		}

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
    	this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, stats.speed);
    }

    public void ToggleState() {
    	if (state == EnemyState.Searching) {
    		state = EnemyState.Attacking;
    	} else {
    		state = EnemyState.Searching;
    	}
    }

    //public void 
}
