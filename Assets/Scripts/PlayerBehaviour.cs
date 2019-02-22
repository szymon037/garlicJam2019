using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public List<Weapon> weaponsData = new List<Weapon>();
    public Weapon activeWeapon = null;
    public const float maximumFireRate = 0.15f;
    public Dictionary<string, int?> ammoCountsForWeapons = new Dictionary<string, int?>();
    public float fireRateTimer = 0f;
    public Transform shootpoint;
    public float hitTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        weaponsData.ForEach(w => ammoCountsForWeapons.Add(w.weaponName, w.infiniteAmmo ? null : (int?)0));
        activeWeapon = weaponsData[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Shoot();
        }

        string input = Input.inputString;

        try {
            ChangeWeapon(System.Int32.Parse(input));
        } catch (System.Exception) {}

        if (fireRateTimer > 0f) {
            fireRateTimer -= Time.deltaTime;
        }

        if (PlayerStats.GetInstance().timers["isHit"] > 0f) {
            PlayerStats.GetInstance().timers["isHit"] -= Time.deltaTime;
        } else {
            PlayerStats.GetInstance().ToggleFlag("isHit", false);
        }

        UpdateCooldowns();

    }

    public void Shoot() {
        if (!this.activeWeapon.infiniteAmmo) {
            if (ammoCountsForWeapons[activeWeapon.weaponName] > 0) ammoCountsForWeapons[activeWeapon.weaponName]--;
        }
        else {
            if (ammoCountsForWeapons[this.activeWeapon.weaponName] <= 0) {
                ammoCountsForWeapons[this.activeWeapon.weaponName] = 0;
                return;
            }
        }
        this.fireRateTimer = Mathf.Clamp(this.activeWeapon.baseFireRate - (float)PlayerStats.GetInstance().stats.fireRateBonus, maximumFireRate, 100f);
        /*spawn a bullet*/
        GameObject bullet = Instantiate(this.activeWeapon.bulletPrefab, shootpoint.position, Quaternion.identity) as GameObject;
        bullet.GetComponent<Bullet>().Init(this.activeWeapon.baseDamage, this.activeWeapon.bulletSpeed, transform.forward);
    }

    public void ChangeWeapon(int index) {
        if (index < 0 || index >= this.weaponsData.Count) return; 
        if (ammoCountsForWeapons[activeWeapon.weaponName] <= 0) return;
        this.fireRateTimer = 0f;
        this.activeWeapon = weaponsData[index];
    }

    public void UpdateCooldowns() {
        foreach (var key in PlayerStats.GetInstance().timers.Keys) {
            if (PlayerStats.GetInstance().timers[key] > 0f) PlayerStats.GetInstance().timers[key] -= Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("WeaponPickup")) {
            Destroy(other.gameObject);
            this.ammoCountsForWeapons[other.gameObject.GetComponent<WeaponPickup>().weaponName] += other.gameObject.GetComponent<WeaponPickup>().ammoValue;
            this.ammoCountsForWeapons[other.gameObject.GetComponent<WeaponPickup>().weaponName] = (int)Mathf.Clamp((int)this.ammoCountsForWeapons[other.gameObject.GetComponent<WeaponPickup>().weaponName], 0, this.activeWeapon.maxAmmo);
        }
    }
}
