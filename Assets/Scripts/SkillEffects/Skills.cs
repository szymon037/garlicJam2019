using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Skills : MonoBehaviour
{
	public static float fearRadius = 5f;
	public static float fearDuration = 15f;

	public static float bombRadius = 5f;
	public static float bombDamage = 25f;
	public static float bombTimer = 2.5f;

	public static void OnionsSadness(Vector3 origin, Vector3 target) {
		Debug.Log("sadness called");
		Collider[] caughtEnemies = (Physics.OverlapSphere(origin, fearRadius) as IEnumerable<Collider>).Where(obj => obj.gameObject.CompareTag("Enemy")).ToArray();
		foreach (var enemy in caughtEnemies) {
			/*make them run away*/
			enemy.gameObject.GetComponent<EnemyAI>().BeginRunAway(fearDuration);
		}
		Skill s = PlayerStats.GetInstance().skills.Where(x => x.skillName == "OnionsSadness").ToArray()[0];
		PlayerStats.GetInstance().SetCooldownTimer(s.skillName, s.cooldown);
	}

	public static void OnionBomb(Vector3 origin, Vector3 target) {
		(Instantiate(Resources.Load("Prefabs/onionbomb") as GameObject, origin, Quaternion.identity) as GameObject).GetComponent<OnionBomb>().BombInit(bombRadius, bombDamage, bombTimer);
		Skill s = PlayerStats.GetInstance().skills.Where(x => x.skillName == "OnionBomb").ToArray()[0];
		PlayerStats.GetInstance().SetCooldownTimer(s.skillName, s.cooldown);
	}

	public static void SummonLeek(Vector3 origin, Vector3 target) {
		string[] enumNames = System.Enum.GetNames(typeof(LeekType));
		string _nameToSpawn = enumNames[Random.Range(0, enumNames.Length)];
		string nameToSpawn = _nameToSpawn;
		nameToSpawn += "Leek";
		//Instantiate(leek)
		GameObject leek = Resources.Load("Prefabs/" + nameToSpawn) as GameObject;
		(Instantiate(leek, origin, Quaternion.identity) as GameObject).GetComponentInChildren<Leek>().Init((LeekType)System.Enum.Parse(typeof(LeekType), _nameToSpawn));
		Skill s = PlayerStats.GetInstance().skills.Where(x => x.skillName == "SummonLeek").ToArray()[0];
		PlayerStats.GetInstance().SetCooldownTimer(s.skillName, s.cooldown);
	}
}
