using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Shoots a bullet based on input from a point on the model */
public class PlayerWeapon : MonoBehaviour
{
    public Transform firePoint; // Point where the shot is fired from
    public GameObject bulletPrefab; // The bullet that is shot
    private Animator animator; // Animation manager 
   
    void Awake()
    {
        animator = GetComponent<Animator>(); 
    }

    // Fires a bullet when 'z' is pressed on the keyboard
    void Update()
    {
        if (Input.GetKeyDown("z"))
        {
            animator.SetBool("Fire", true); // Plays the firing animation
        }
    }

    // Animation event enters at the end of the firing animation
    // Creates a bullet copy at the firing point
    public void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        animator.SetBool("Fire", false);
    }
}
