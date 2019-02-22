using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flags = System.Collections.Generic.Dictionary<string, bool>;
using System.Reflection;
using Timers = System.Collections.Generic.Dictionary<string, float>;

public struct Attributes {
	public float speed;
	public float fireRateBonus;
	public float damageBonus;
	public float health;
	public float maxHealth;

	public Attributes(float _speed) {
		speed = _speed;
		fireRateBonus = 0f;
		health = maxHealth = 150f;
		damageBonus = 0f;
	}	
}

public struct Skill {
	public System.Action<Vector3, Vector3> usage;
	public string skillName;
	public float cooldown;
	public float? damage;

	public Skill(string _name, float _cooldown, float? _damage) {
		this.usage = typeof(Skills).GetMethod(_name, BindingFlags.Static | BindingFlags.Public).CreateDelegate(typeof(System.Action<Vector3, Vector3>)) as System.Action<Vector3, Vector3>;
		this.skillName = _name;
		this.cooldown = _cooldown;
		this.damage = _damage;
	}
}



public class PlayerStats 
{
	private static PlayerStats instance = null;

	public Attributes stats;
	public int currency;
	public Flags flags;

	public Timers timers = new Timers();

	public List<Skill> skills;

	public static PlayerStats GetInstance() {
		if (instance == null) instance = new PlayerStats();
		return instance;
	}

	private PlayerStats() {
		stats = new Attributes();
		currency = 0;
		flags = new Flags();
		flags.Add("isHit", false);
		skills = new List<Skill>();
		timers.Add("isHit", 0f);
		foreach (var skill in skills) {
			timers.Add(skill.skillName, 0f);
		}
	}	

	public void ChangeHealth(float value) {
		this.stats.health += value;
		if (this.stats.health > this.stats.maxHealth) this.stats.health = this.stats.maxHealth;
		if (this.stats.health < 0f) this.stats.health = 0f;
	}

	public void ToggleFlag(string key, bool value, float timerValue = 0f) {
		if (!this.flags.ContainsKey(key)) throw new System.InvalidOperationException("no such flag in dictionary");
		this.flags[key] = value;
		if (timerValue != 0f) {
			timers[key] = timerValue;
		}
	}

	public void SetCooldownTimer(string skillName, float timerValue) {
		this.timers[skillName] = timerValue;
	}
}
