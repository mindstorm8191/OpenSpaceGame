using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy1 : MonoBehaviour
{
    float health;
    // Start is called before the first frame update
    void Start()
    {
        health = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(float amount) {
        health -= amount;
        if(health <= 0) {

            // Let's spawn some kind of death thing... but what? oh I don't know - oh! A sphere! That'll do it!
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = transform.position;
            // There! This represents the death animation of the enemy

            Destroy(gameObject);

        }
        Debug.Log(health);
    }
}
