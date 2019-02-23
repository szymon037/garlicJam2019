//#define DEBUG
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{
    public List<Weapon> weaponsData = new List<Weapon>();
    public Weapon activeWeapon = null;
    public const float maximumFireRate = 0.15f;
    public Dictionary<string, int?> ammoCountsForWeapons = new Dictionary<string, int?>();
    public float fireRateTimer = 0f;
    public Transform shootpoint;
    public float hitTimer = 0f;
    public Vector3 moveDir = Vector3.zero;
    public Rigidbody rb = null;
    public Camera tpsCamera = null;
    public Vector3 offset = new Vector3(4f, 1f, 4f);
    public float rotationspeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionY;
        weaponsData.ForEach(w => ammoCountsForWeapons.Add(w.weaponName, w.infiniteAmmo ? null : (int?)0));
        activeWeapon = weaponsData[0];
        //tpsCamera.transform.position = transform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {   
        Debug.DrawRay(this.transform.position, transform.forward * 20, Color.red, 5f);
        //tpsCamera.transform.position = transform.position + offset;
        //tpsCamera.transform.LookAt(this.transform);
        if (Input.GetMouseButton(0)) {
            Shoot();
        }

        float x, z;
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        transform.Rotate(0f, rotationspeed * Input.GetAxis("Mouse X"), 0f);

        //transform.Translate(x * PlayerStats.GetInstance().stats.speed * Time.deltaTime, 0, z * PlayerStats.GetInstance().stats.speed * Time.deltaTime);

        // rb.velocity = (!Mathf.Approximately(x, 0f) && !Mathf.Approximately(z, 0f) ? new Vector3(x, 0f, z) : transform.forward * PlayerStats.GetInstance().stats.speed);
        rb.velocity = Vector3.zero;
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) /*&& !isRotating*/)
        {
            rb.velocity += transform.forward * PlayerStats.GetInstance().stats.speed;
            Debug.Log("AAAA");
        }
        if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) /*&& !isRotating*/)
        {
            rb.velocity += -transform.forward * PlayerStats.GetInstance().stats.speed;
        }
        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))/* && !isRotating*/)
        {
            rb.velocity += -transform.right * PlayerStats.GetInstance().stats.speed;
        }
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) /*&& !isRotating*/)
        {
            rb.velocity += transform.right * PlayerStats.GetInstance().stats.speed;
        }

        string input = Input.inputString;

        try {
            ChangeWeapon(System.Int32.Parse(input));
        } catch (System.Exception) {}

        if (fireRateTimer > 0f) {
            fireRateTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Z) && PlayerStats.GetInstance().timers["OnionsSadness"] <= 0f) {
            PlayerStats.GetInstance().FindSkillWithName("OnionsSadness").usage(this.transform.position, Vector3.zero);
        } else if (Input.GetKeyDown(KeyCode.X) && PlayerStats.GetInstance().timers["OnionBomb"] <= 0f) {
            PlayerStats.GetInstance().FindSkillWithName("OnionBomb").usage(this.transform.position, Vector3.zero);
        } else if (Input.GetKeyDown(KeyCode.C) && PlayerStats.GetInstance().timers["SummonLeek"] <= 0f) {
            PlayerStats.GetInstance().FindSkillWithName("SummonLeek").usage(this.transform.position + new Vector3(2f, 0f, 2f), Vector3.zero);
        }

        if (PlayerStats.GetInstance().timers["isHit"] > 0f) {
            PlayerStats.GetInstance().timers["isHit"] -= Time.deltaTime;
        } else {
            PlayerStats.GetInstance().ToggleFlag("isHit", false);
        }

        UpdateCooldowns();

        DebugTimers();

    }

    public void Shoot() {
        if (this.fireRateTimer > 0f) return;
        if (this.activeWeapon == null) return;
        if (!this.activeWeapon.infiniteAmmo) {
            if (ammoCountsForWeapons[activeWeapon.weaponName] > 0) ammoCountsForWeapons[activeWeapon.weaponName]--;
        }
        else {
            if (ammoCountsForWeapons[this.activeWeapon.weaponName] <= 0) {
                ammoCountsForWeapons[this.activeWeapon.weaponName] = 0;
                return;
            }
        }
        this.fireRateTimer = Mathf.Clamp(this.activeWeapon.baseFireRate - PlayerStats.GetInstance().stats.fireRateBonus, maximumFireRate, 100f);
        /*spawn a bullet*/
        GameObject bullet = Instantiate(this.activeWeapon.bulletPrefab, shootpoint.position, Quaternion.identity) as GameObject;
        bullet.GetComponent<Bullet>().Init(this.activeWeapon.baseDamage + PlayerStats.GetInstance().stats.damageBonus, this.activeWeapon.bulletSpeed, transform.right);
    }

    public void ChangeWeapon(int index) {
        if (index < 0 || index >= this.weaponsData.Count) return; 
        if (ammoCountsForWeapons[activeWeapon.weaponName] <= 0) return;
        this.fireRateTimer = 0f;
        this.activeWeapon = weaponsData[index];
    }

    public void UpdateCooldowns() {
        foreach (var key in PlayerStats.GetInstance().timers.Keys.ToList()) {
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

    public void DebugTimers() {
        foreach (var t in PlayerStats.GetInstance().timers) {
            Debug.Log(string.Format("{0}, {1}", t.Key, t.Value));
        }
    }
}
