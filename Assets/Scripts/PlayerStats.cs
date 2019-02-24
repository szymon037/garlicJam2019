using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flags = System.Collections.Generic.Dictionary<string, bool>;
using System.Reflection;
using Timers = System.Collections.Generic.Dictionary<string, float>;

public class Attributes {
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

	public override string ToString() {
		string result = "";
		result += "Speed: " + System.Math.Round(speed, 2).ToString();
		result += '\n';
		result += "Fire rate bonus: " + System.Math.Round(fireRateBonus, 2).ToString();
		result += '\n';
		result += "Damage bonus: " + System.Math.Round(damageBonus, 2).ToString();
		result += '\n';
		result += "Max Health: " + System.Math.Round(maxHealth, 0).ToString();
		return result;
	}	
}

public class Skill {
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

public enum CurrentBiome {
	Forest = 0,
	Eerie,
	Snowy,
	Volcanic
}

public class PlayerStats 
{
	private static PlayerStats instance = null;

	public Attributes stats;
	public int currency;
	public Flags flags;

	public Timers timers = new Timers();
	public int score;

	public List<Skill> skills;

	public static PlayerStats GetInstance() {
		if (instance == null) instance = new PlayerStats();
		return instance;
	}

	private PlayerStats() {
		stats = new Attributes(12f);
		currency = 0;
		score = 0;
		flags = new Flags();
		flags.Add("isHit", false);
		skills = new List<Skill>();
		timers.Add("isHit", 0f);
		

		Dictionary<string, KeyValuePair<float, float?>> skillData = new Dictionary<string, KeyValuePair<float, float?>>() {
			{"OnionsSadness", new KeyValuePair<float, float?>(15f, null)},
			{"OnionBomb", new KeyValuePair<float, float?>(30f, 25f)},
			{"SummonLeek", new KeyValuePair<float, float?>(45f, null)}
		};
		
		foreach (var entry in skillData) {
			skills.Add(new Skill(entry.Key, entry.Value.Key, entry.Value.Value));
		}

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
		Debug.Log("im here");
	}

	public void AcquireCurrency(int amount) {
		this.currency += amount;
	}

	public Skill FindSkillWithName(string s) {
		foreach (var skill in skills) {
			if (s == skill.skillName) return skill;
		}

		return null;
	}

	public void UpgradeAttribute(string attrName, int currencyCost) {
		if (currency < currencyCost) return;
		else AcquireCurrency(-currencyCost);
		FieldInfo field = null;
		// switch (attrName) {
		// 	case "speed":
		// 		field = stats.GetType().GetField(attrName, BindingFlags.Instance | BindingFlags.Public);
		// 		field.SetValue(stats, (float)field.GetValue(stats) + 1f);
		// 		break;
		// 	case "fireRateBonus":
		// 		field = stats.GetType().GetField(attrName, BindingFlags.Instance | BindingFlags.Public);
		// 		field.SetValue(stats, (float)field.GetValue(stats) + 0.05f);
		// 		break;
		// 	case "damageBonus":
		// 		field = stats.GetType().GetField(attrName, BindingFlags.Instance | BindingFlags.Public);
		// 		field.SetValue(stats, (float)field.GetValue(stats) + 2.5f);
		// 		break;
		// 	case "health":
		// 		field = stats.GetType().GetField(attrName, BindingFlags.Instance | BindingFlags.Public);
		// 		FieldInfo anotherField = stats.GetType().GetField("maxHealth", BindingFlags.Instance | BindingFlags.Public);
		// 		anotherField.SetValue(stats, (float)anotherField.GetValue(stats) + 15f);
		// 		field.SetValue(stats, (float)field.GetValue(stats) + 15f);
		// 		break;
		// }
		if (attrName == "health") {
			FieldInfo f = stats.GetType().GetField("maxHealth", BindingFlags.Instance | BindingFlags.Public);
			f.SetValue(stats, (float)f.GetValue(stats) + UpgradeManager.upgradeValues[attrName]);
		}
		field = stats.GetType().GetField(attrName, BindingFlags.Instance | BindingFlags.Public);
		field.SetValue(stats, (float)field.GetValue(stats) + UpgradeManager.upgradeValues[attrName]);
		Debug.Log(stats.ToString());
	}
}
