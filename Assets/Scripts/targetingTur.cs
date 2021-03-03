using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetingTur : MonoBehaviour
{
    private Transform target;

    [Header("Attributes")]

    public float range =2.5f;
    public float fireRate = 1f;
    public float fireCountdown = 0f;

    [Header("Unity Setup Fields")]

    public string enemyTag = "enemy";
    public Transform partToRotate;
    public float turnSpeed = 8f;

    public GameObject bulletPrefab;
    public Transform firePoint;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

        }
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;
        /**Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir, Vector3.up);
        Vector3 rotation = lookRotation.eulerAngles;
         partToRotate.rotation = Quaternion.Euler(0f, 0f, rotation.x + rotation.y);
        **/
        var relativePos = target.position - transform.position;
        var angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(partToRotate.rotation, rotation, Time.deltaTime * turnSpeed);

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }
    void Shoot()
    {
        GameObject bulletGo = (GameObject)Instantiate (bulletPrefab, firePoint.position, firePoint.rotation);
        hitDetection bullet = bulletGo.GetComponent<hitDetection>();

        if (bullet != null)
            bullet.Seek(target);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
