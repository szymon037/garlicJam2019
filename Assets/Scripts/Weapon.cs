using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
	public string weaponName;
   	public float baseFireRate;
   	public float baseDamage;
   	public float bulletSpeed;
   	public int maxAmmo;
   	public bool infiniteAmmo;
  // 	public bool explosive;
   	public Sprite weaponSprite;
   	public GameObject bulletPrefab;
   	public Color bulletColor;
}
