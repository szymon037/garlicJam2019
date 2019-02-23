using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LeekType : byte {
	Spring,
	Summer,
	Fall,
	Winter
}

public struct LeekStats {
	public float health;
	public float fireRate;
	public float speed;

	public LeekStats(float _health, float _fireRate, float _speed) {
		health = _health;
		fireRate = _fireRate;
		speed = _speed;
	}
}

[RequireComponent(typeof(ParticleSystem))]
public class Leek : MonoBehaviour
{
	public static Dictionary<LeekType, LeekStats> leekTypeStats = new Dictionary<LeekType, LeekStats>() {
		{LeekType.Spring, new LeekStats(100f, 0.35f, 5f)},
		{LeekType.Summer, new LeekStats(133f, 0.28f, 6.5f)},
		{LeekType.Fall, new LeekStats(150f, 0.21f, 8f)},
		{LeekType.Winter, new LeekStats(166f, 0.14f, 10f)}
	};

	public LeekStats stats = new LeekStats();
	public ParticleSystem ps;

	void Start()
	{
		ps = gameObject.GetComponent<ParticleSystem>();
		ps.Play();
	}

	void Update() {

	}
    

    public void Init(LeekType type) {
    	this.stats = leekTypeStats[type];
    }
}
