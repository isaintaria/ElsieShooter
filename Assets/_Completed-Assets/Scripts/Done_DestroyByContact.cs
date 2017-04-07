using UnityEngine;
using System.Collections;
using System;

public class Done_DestroyByContact : MonoBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
    public int objectHp;
	public int scoreValue;
    private int hp;
	private Done_GameController gameController;
    private EffectManager effectManager;

    void Start ()
	{
        effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
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
		else if( tag == "Enemy" && other.tag == "Beam")
        {
            KillEnemyByPlayerMissile(other);   
        }
        else if ( tag == "Item" && other.tag == "Player")
        {
            Debug.Log(gameObject.name);
            gameController.BombCount++;
            gameController.GetComponents<AudioSource>()[1].Play();          
            Destroy(gameObject);
        }

		if (other.tag == "Player" && tag == "Enemy")
		{     
            if( !gameController.IsPlayerDead )
            {
                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                //gameController.GameOver();
                //gameController.AddScore(scoreValue);
                gameController.PlayerDead();
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
			
        }
		
		
	}

    private void KillEnemyByPlayerMissile(Collider other)
    {
        if (hp > 1)
        {
            hp--;
            Destroy(other.gameObject);
            return;
        }
        //폭발 시킨다.
        if (explosion != null)
        {
            if( gameObject.name.Contains("Green") )
            {
                Debug.Log("상자 생성");
                Instantiate(gameController.itemObject, transform.position, transform.rotation);
            }            
            Instantiate(explosion, transform.position, transform.rotation);
            effectManager.FireEnemyDeadByPlayerBeamEffect();
            gameController.AddScore(scoreValue);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}