using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Skills : MonoBehaviour
{
	public static float fearRadius = 5f;
	public static float fearDuration = 10f;
	public static void OnionsSadness(Vector3 origin, Vector3 target = default(Vector3)) {
		Collider[] caughtEnemies = (Physics.OverlapSphere(origin, fearRadius) as IEnumerable<Collider>).Where(obj => obj.gameObject.CompareTag("Enemy")).ToArray();
		foreach (var enemy in caughtEnemies) {
			/*make them run away*/
			enemy.gameObject.GetComponent<EnemyAI>().BeginRunAway(fearDuration);
		}
		Skill s = PlayerStats.GetInstance().skills.Where(x => x.skillName == "OnionsSadness").ToArray()[0];
		PlayerStats.GetInstance().SetCooldownTimer(s.skillName, s.cooldown);
	}
}
