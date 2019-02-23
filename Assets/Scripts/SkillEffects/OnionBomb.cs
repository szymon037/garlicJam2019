using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//[RequireComponent(typeof(ParticleSystem))]
public class OnionBomb : MonoBehaviour
{
    public float damage;
    public float radius;
    public ParticleSystem[] ps;
    float timer = 2.5f;
    bool initflag = false;
    public GameObject sparks;
    public GameObject smoke;
    public GameObject audioSource;
    
    void Start() {
        smoke.gameObject.SetActive(true);
        smoke.gameObject.GetComponent<ParticleSystem>().Play();
    }

    void Update() {
        if (timer > 0f) {
            timer -= Time.deltaTime;
        } else {
            try {
                Explode();
            } catch (System.Exception) {
            
            }
            
            Destroy(smoke, 3f);
            Destroy(sparks, 3f);
            audioSource.GetComponent<AudioSource>().Play();
            sparks.gameObject.SetActive(true);
            sparks.transform.SetParent(null);
            audioSource.transform.SetParent(null);
            sparks.gameObject.GetComponent<ParticleSystem>().Play();
            Destroy(audioSource, 3f);
            Destroy(this.gameObject);
        }
    }

    public void BombInit(float radius, float damage, float detonationTime) {
    	this.radius = radius;
    	this.damage = damage;
        timer = detonationTime;
        initflag = true;
    	//Destroy(this.gameObject, detonationTime);
    }

    public void Explode() {
    	Collider[] enemies = (Physics.OverlapSphere(this.transform.position, radius) as IEnumerable<Collider>).Where(x => x.gameObject.CompareTag("Enemy")).ToArray();
        if (enemies.Length ==0) return;
    	foreach (var enemy in enemies) {
    		enemy.gameObject.GetComponent<EnemyAI>().ReceiveDamage(damage);
    	}
    }
}
