using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Slice : MonoBehaviour
{
	public GameObject trail;
	public Detection detection;
	public Score score;
	public float speed = 5f;
	public bool isSecond = false;
	bool isCutting = false;
	Rigidbody2D rb;
	CircleCollider2D _collider;
	Camera cam;
	Vector3 pos;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		cam = Camera.main;
		_collider = GetComponent<CircleCollider2D>();
	}
	private void Update()
	{
		float gDist, oDist;
		StartCutting();
		if(!isSecond)
		{
			gDist = Vector3.Distance(pos, detection.gPosition);
			if (gDist < 25f)
				_collider.enabled = false;
			pos = Vector3.MoveTowards(pos, detection.gPosition, gDist * speed * Time.deltaTime);
		}
		if(isSecond)
		{
			oDist = Vector3.Distance(pos, detection.oPosition);
			if (oDist < 25f)
				_collider.enabled = false;
			pos = Vector3.MoveTowards(pos, detection.oPosition, oDist * speed * Time.deltaTime);
		}
		//pos = detection.position;
		pos.z = 15f;
		
		rb.position = cam.ScreenToWorldPoint(pos);
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (isSecond && collision.gameObject.name.Substring(0, 6) == "Second")
		{
			collision.collider.gameObject.SetActive(false);
			score.score++;
		}
		else if (!isSecond && collision.gameObject.name.Substring(0, 5) == "First")
		{
			collision.collider.gameObject.SetActive(false);
			score.score++;
		}
	}
	
	void StartCutting()
	{
		isCutting = true;
		_collider.enabled = true;
		trail.SetActive(true);
	}
/*	void StopCutting()
	{
		_collider.enabled = false;
		trail.SetActive(false);
		isCutting = false;
	}*/
}