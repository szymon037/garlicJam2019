using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public float spawnTimer = 0f;
	public GameObject garlic;
	public float newAngleZ;
	public static int enemies = 0;
	public static int maxEnemies = 150;
    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = 0.25f;

    }

    // Update is called once per frame
    void Update()
    {
        if (spawnTimer > 0f) spawnTimer -= Time.deltaTime;
        newAngleZ = transform.eulerAngles.z;
         if (/*transform.rotation.z*/newAngleZ >= -0.5f && /*transform.rotation.z*/newAngleZ <= 0.5f)
        {
	        
	        if (spawnTimer <= 0f&& enemies < maxEnemies) {
	        	spawnTimer = 0.75f * 2f;
	        	GameObject g = Instantiate(garlic, this.transform.position + new Vector3(3f, 0f, 3f), Quaternion.identity) as GameObject;
	        	g.GetComponentInChildren<EnemyAI>().SetAngle(0f);
	        	enemies++;
	        }
        }
        else if (/*transform.rotation.z*/newAngleZ >= -90.5f && /*transform.rotation.z*/newAngleZ <= -89.5f || /*transform.rotation.z*/newAngleZ >= 269.5 &&/* transform.rotation.z*/newAngleZ <= 270.5)
        {
           if (spawnTimer <= 0f && enemies < maxEnemies) {
	        	spawnTimer = 0.75f * 2f;
	        	GameObject g = Instantiate(garlic, this.transform.position + new Vector3(3f, 0f, 3f), Quaternion.identity) as GameObject;
	        	g.GetComponentInChildren<EnemyAI>().SetAngle(-90f);enemies++;
	        	// Destroy(g.GetComponentInChildren<Rigidbody>());
	        }
        }
        else if (/*transform.rotation.z*/newAngleZ >= -180.5f && /*transform.rotation.z*/newAngleZ <= -179.5f || /*transform.rotation.z*/newAngleZ >= 179.5f && /*transform.rotation.z*/newAngleZ <= 180.5f)
        {
          if (spawnTimer <= 0f&& enemies < maxEnemies) {
	        	spawnTimer = 0.75f * 2f;
	        	GameObject g = Instantiate(garlic, this.transform.position + new Vector3(3f, 0f, 3f), Quaternion.identity) as GameObject;
	        	g.GetComponentInChildren<EnemyAI>().SetAngle(-180f);enemies++;
	        	//Destroy(g.GetComponentInChildren<Rigidbody>());
	        }
        }
        else if (/*transform.rotation.z*/newAngleZ >= -270.5f && /*transform.rotation.z*/newAngleZ <= -269.5f || /*transform.rotation.z*/newAngleZ >= 89.5f && /*transform.rotation.z*/newAngleZ <= 90.5f)
        {
            if (spawnTimer <= 0f&& enemies < maxEnemies) {
	        spawnTimer = 0.75f * 2f;
	        	GameObject g = Instantiate(garlic, this.transform.position + new Vector3(3f, 0f, 3f), Quaternion.identity) as GameObject;
	        	g.GetComponentInChildren<EnemyAI>().SetAngle(-270f);enemies++;
	        	// Destroy(g.GetComponentInChildren<Rigidbody>());
	        }
        }
    }
}
