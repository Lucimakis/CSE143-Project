using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject death;
    private SpriteRenderer sr;
    private int health = 100;
    public float damagedTime = 0.2f;
    public float deathAnim = 1.0f;

    // Start is called before the first frame update

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    public void Damage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
        StartCoroutine(switchColor());
    }

    // Update is called once per frame
    void Die()
    {
        Destroy(gameObject);
        GameObject body = (GameObject)Instantiate(death, transform.position, transform.rotation);
        Destroy(body, deathAnim);
    }

    IEnumerator switchColor()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(damagedTime);
        sr.color = Color.white;
    }
}
