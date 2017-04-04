using UnityEngine;
using System.Collections;

public class Done_DestroyByContact : MonoBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
    public int objectHp;
	public int scoreValue;
    private int hp;
	private Done_GameController gameController;

	void Start ()
	{
        hp = objectHp;
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <Done_GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Boundary" || other.tag == "Enemy")
		{
			return;
		}
		if( tag == "Enemy" && other.tag == "Beam")
        {
            if( hp > 1 )
            {
                hp--;
                Destroy(other.gameObject);
                return;
            }
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
                gameController.AddScore(scoreValue);
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
                          
        }

		if (other.tag == "Player")
		{
			Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
			gameController.GameOver();

            gameController.AddScore(scoreValue);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
		
		
	}
}