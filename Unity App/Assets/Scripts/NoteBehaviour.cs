using UnityEngine;
using System.Collections;

public class NoteBehaviour : MonoBehaviour {

	public float speed = 5f;
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Translate(Vector3.back * speed *Time.deltaTime);	
	}
}
