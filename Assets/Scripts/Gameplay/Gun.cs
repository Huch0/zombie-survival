using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Scripts.Game;

namespace Unity.Scripts.Gameplay
{
    public class Gun : MonoBehaviour
    {
        public int ammoCapacity = 30;
        public float shootingSpeed = 1f;
        public int damage = 1;
        private int currentAmmo;
        private float lastShootTime;
        [SerializeField]
        float bulletSpeed = 20f;
        [SerializeField]
        public float bulletRadius = 0.01f;

        void Start()
        {
            currentAmmo = ammoCapacity;
            lastShootTime = -shootingSpeed;
        }

        public void Shoot()
        {
            if (Time.time - lastShootTime >= shootingSpeed && currentAmmo > 0)
            {
                lastShootTime = Time.time;
                currentAmmo--;
                Debug.Log("Shooting");
                SpawnBullet();
            }
            else if (currentAmmo <= 0)
            {
                Debug.Log("Out of ammo, reload!");
            }
        }

        public void Reload()
        {
            currentAmmo = ammoCapacity;
            Debug.Log("Reloading");
        }

        void SpawnBullet()
        {
            GameObject bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bullet.transform.position = this.transform.position;
            bullet.transform.rotation = this.transform.rotation;
            bullet.transform.localScale = new Vector3(bulletRadius, bulletRadius, bulletRadius);
            bullet.AddComponent<Bullet>().SetSpeed(bulletSpeed).SetDamage(damage);

            Rigidbody rb = bullet.AddComponent<Rigidbody>();
            rb.useGravity = false;

            SphereCollider sc = bullet.GetComponent<SphereCollider>();
            sc.radius = bulletRadius;
        }
    }
}

public class Bullet : MonoBehaviour
{
    public float speed;
    public int damage;

    public Bullet SetSpeed(float bulletSpeed)
    {
        this.speed = bulletSpeed;
        return this;
    }

    public Bullet SetDamage(int bulletDamage)
    {
        this.damage = bulletDamage;
        return this;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject hitObject = collision.gameObject;

        Debug.Log("Hit object");

        Health health = hitObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage, this.gameObject);
        }

        Destroy(this.gameObject);
    }
}