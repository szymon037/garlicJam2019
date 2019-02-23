using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(ParticleSystem))]
public class OnionBomb : MonoBehaviour
{
    public float damage;
    public float radius;
    public ParticleSystem ps;

    void OnDestroy() {
    	/*particle effect here*/
    	Explode();
    	ps = gameObject.GetComponent<ParticleSystem>();
    	ps.Play();
    }

    public void BombInit(float radius, float damage, float detonationTime) {
    	this.radius = radius;
    	this.damage = damage;
    	Destroy(this.gameObject, detonationTime);

    }

    public void Explode() {
    	Collider[] enemies = (Physics.OverlapSphere(this.transform.position, radius) as IEnumerable<Collider>).Where(x => x.gameObject.CompareTag("Enemy")).ToArray();
    	foreach (var enemy in enemies) {
    		enemy.gameObject.GetComponent<EnemyAI>().ReceiveDamage(damage);
    	}
    }
}
