using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public float spawnTimer = 0f;
	public GameObject garlic;
	public float newAngleZ;
	public static int enemies = 0;
	public static int maxEnemies = 15;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        enemies = 0;
        player = GameObject.FindWithTag("Player").transform;
        spawnTimer = 0.25f;

    }
    public void Update(){
    	if (Vector3.Distance(player.position, transform.position) < 40f && spawnTimer > 0f) spawnTimer-=Time.deltaTime;
        newAngleZ = transform.eulerAngles.z;
        if (newAngleZ >= -0.5f && newAngleZ <= 0.5f)
        {	        
	        if (spawnTimer <= 0f && enemies < maxEnemies) {
	        	spawnTimer = Random.Range(/*0.5f*/1.5f, 3.5f);
	        	GameObject g = Instantiate(garlic, this.transform.position + new Vector3(3f, 0f, 3f), Quaternion.identity) as GameObject;
                g.GetComponentInChildren<EnemyAI>().SetAngle(0f);
                g.GetComponentInChildren<EnemyAI>().topBottom = true;
                g.GetComponentInChildren<EnemyAI>().SetAngle(0f);
                g.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                enemies++;
	        }
        }
        else if (newAngleZ >= -90.5f && newAngleZ <= -89.5f || newAngleZ >= 269.5 &&newAngleZ <= 270.5)
        {
           if (spawnTimer <= 0f && enemies < maxEnemies) {
                spawnTimer = Random.Range(/*0.5f*/1.5f, 3.5f);
                GameObject g = Instantiate(garlic, this.transform.position + new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
	        	g.GetComponentInChildren<EnemyAI>().SetAngle(0f);
                g.GetComponentInChildren<EnemyAI>().rightLeft = true;
                //g.GetComponent<Transform>().Rotate(new Vector3(0f, 0f, -90f), Space.World);
                g.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                enemies++;
	        	// Destroy(g.GetComponentInChildren<Rigidbody>());
	        }
        }
        else if (newAngleZ >= -180.5f && newAngleZ <= -179.5f || newAngleZ >= 179.5f && newAngleZ <= 180.5f)
        {
          if (spawnTimer <= 0f && enemies < maxEnemies) {
                spawnTimer = Random.Range(/*0.5f*/1.5f, 3.5f);
                GameObject g = Instantiate(garlic, this.transform.position + new Vector3(3f, 0f, 3f), Quaternion.identity) as GameObject;
	        	g.GetComponentInChildren<EnemyAI>().SetAngle(-180f);
                g.GetComponentInChildren<EnemyAI>().topBottom = true;
                g.GetComponent<Transform>().Rotate(new Vector3(0f, 0f, -180f), Space.World);
                enemies++;
	        	//Destroy(g.GetComponentInChildren<Rigidbody>());
	        }
        }
        else if (newAngleZ >= -270.5f && newAngleZ <= -269.5f || newAngleZ >= 89.5f && newAngleZ <= 90.5f)
        {
            if (spawnTimer <= 0f && enemies < maxEnemies) {
                spawnTimer = Random.Range(/*0.5f*/1.5f, 3.5f);
                GameObject g = Instantiate(garlic, this.transform.position + new Vector3(3f, 0f, 3f), Quaternion.identity) as GameObject;
	        	g.GetComponentInChildren<EnemyAI>().SetAngle(-180f);
                g.GetComponentInChildren<EnemyAI>().rightLeft = true;
                g.GetComponent<Transform>().Rotate(new Vector3(0f, 0f, -180f), Space.World);
                enemies++;
	        	// Destroy(g.GetComponentInChildren<Rigidbody>());
	        }
        }
    }
}
