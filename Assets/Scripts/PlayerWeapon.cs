using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{

    public Transform firePoint;
    public GameObject bulletPrefab;
    public Animator animator;
   
    void Update()
    {
        if (Input.GetKeyDown("z"))
        {
            animator.SetBool("Fire", true);
        }
    }

    public void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        animator.SetBool("Fire", false);
    }
}
