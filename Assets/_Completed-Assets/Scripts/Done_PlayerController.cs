using UnityEngine;
using System.Collections;

[System.Serializable]
public class Done_Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

public class Done_PlayerController : MonoBehaviour
{
	public float speed;
	public float tilt;
	public Done_Boundary boundary;

	public GameObject shot;
	public Transform shotSpawn;
    public GameObject bombEffect;
    public Transform bombSpawn;
    public float fireRate;
    public float bombCoolTime;
	 
	private float nextFire;
    private float nextBomb;


    private Done_GameController gameController;
	
	void Update ()
	{
		if (Input.GetButton("Fire1") && Time.time > nextFire) 
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			GetComponent<AudioSource>().Play ();
		}
        else if( Input.GetButton("Fire2") && Time.time > nextBomb )
        {
            if (gameController.BombCount <= 0)
                return;
            gameController.BombCount--;
            nextBomb = Time.time + bombCoolTime;
            Instantiate(bombEffect, bombSpawn.position, bombSpawn.rotation);            
            GetComponent<AudioSource>().Play();
            Bomb_Action();
        }
	}

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<Done_GameController>();
    }

    void Bomb_Action()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
        foreach( GameObject obj in gos )
        {
            Destroy(obj);
        }
        
    }

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody>().velocity = movement * speed;
		
		GetComponent<Rigidbody>().position = new Vector3
		(
			Mathf.Clamp (GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp (GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
		);
		
		GetComponent<Rigidbody>().rotation = Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
	}
}
