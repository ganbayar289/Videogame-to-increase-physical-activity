using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public Rigidbody2D[] objects;
    public Vector3 speed;
    Vector3 position;
    Quaternion rotation;
    private float timeInBetween = 1f;

    void Start()
    {
        position = transform.position;
        rotation = transform.rotation;
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
	{
        while(true)
		{
            position = new Vector3(Random.Range(transform.position.x - 9f, transform.position.x + 9f), transform.position.y, 15f);
            speed.x = Random.Range(0f, position.x * -20f);
            speed.y = Random.Range(700, 900);
            rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
            Instantiate(objects[Random.Range(0,objects.Length)], position, rotation).AddForce(speed);
            yield return new WaitForSeconds(Random.Range(timeInBetween - 0.2f, timeInBetween + 0.2f));
        }
	}
}
