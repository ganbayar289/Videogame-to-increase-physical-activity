using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes2D;

public class ObjectSpawner : MonoBehaviour
{
    public Rigidbody2D[] objects;
    public Shape[] shapes;
    public Vector3 speed;
    Vector3 position;
    Quaternion rotation;
    private float timeInBetween = 1f;

    void Start()
    {
        Color sColor = Color.HSVToRGB((float)PlayerPrefs.GetInt("ox") / 180, 1f, 1f);
        Color fColor = Color.HSVToRGB((float)PlayerPrefs.GetInt("gx") / 180, 1f, 1f);
        position = transform.position;
        rotation = transform.rotation;

        foreach(Shape shape in shapes)
		{
            if (shape.name.Substring(0, 5) == "First")
            {
                shape.settings.outlineColor = fColor;
                shape.settings.fillColor = new Color(fColor.r * 0.7f, fColor.g * 0.7f, fColor.b * 0.7f);
            }
            else
            {
                shape.settings.outlineColor = sColor;
                shape.settings.fillColor = new Color(sColor.r * 0.7f, sColor.g * 0.7f, sColor.b * 0.7f) ;
            }
		}
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
