using Assets._Completed_Assets.Scripts.TableManager.TableItems;
using UnityEngine;
using System.IO.Ports;
using System;
using System.Collections;

public class EffectManager : MonoBehaviour {

 
    OptionTable table;
    SerialPort port;
    
  

  
    private void MakeEffect(string loc,EffectOption effect,AudioSource source)
    {
        if (effect.bulb)
            EffectBulb(effect.pattern_b,loc);
        if (effect.vibration)
            EffectVibration(effect.pattern_v,loc);
        if (effect.speaker)
            EffectSpeaker(loc);
        if (source != null)
            source.Play();                                
    }

    private void EffectSpeaker(string str = "Debug")
    {
        Debug.Log(str + "에서  소리 이펙트 발생");
    }

    private  void EffectVibration(int pattern_v,string str ="Debug")
    {
        Debug.Log(str + "에서  진동 이펙트 발생 패턴:" + pattern_v);
    }

    private  void EffectBulb(int pattern_b, string str = "Debug")
    {
        Debug.Log(str + "에서 전구 이펙트 발생 패턴:" +pattern_b);
        StartCoroutine(BulbTest());
      
    }

    private IEnumerator BulbTest()
    {
        port.Write(new char[] { '1' }, 0, 1);
        yield return new WaitForSeconds(0.3f);
        port.Write(new char[] { '0' }, 0, 1);

    }

    public void FirePlayerBeamEffect()
    {
        MakeEffect("FirePlayerBeamEffect()", table.FireBeam, audioBeamFire);
    }


    public void FirePlayerBombExplosionEffect()
    {
        MakeEffect("FirePlayerBombExplosionEffect()", table.Explosion_Bomb, audioExplosionBomb);
    }
    public void FirePlayerBombShootEffect()
    {
        MakeEffect("FirePlayerBombShootEffect()", table.FireBomb, audioBombShoot);
    }
    public void FireEnemyDeadByPlayerBeamEffect()
    {
        MakeEffect("FireEnemyDeadByPlayerBeamEffect()", table.Destroy_Enemy, audioDestroyEnemy);
    }
    public void FirePlayerDeadEffect()
    {
        MakeEffect("FirePlayerDeadEffect()", table.Destroy_Player, audioDestroyPlayer);
    }
    public void FireAstroidExplosionEffect()
    {
        MakeEffect("FireAstroidExplosionEffect()", table.Destroy_Astroid, audioDestroyAstroid);
    }
    public void FireGetBombEffect()
    {
        MakeEffect("FireAstroidExplosionEffect()", table.Destroy_Astroid, audioDestroyAstroid);
    }
    public void FireGetBonusEffect()
    {
        MakeEffect("FireGetBonusEffect()", table.GetBonusScore, audioGetBonusScore);
    }
    public void FireBeamCollisionEffect()
    {
        MakeEffect("FireBeamCollisionEffect()", table.BeamCollision, audioBeamCollision);
    }

    AudioSource audioBeamFire; // 빔발사
    AudioSource audioBombShoot; // 폭탄발사
    AudioSource audioExplosionBomb; // 폭탄폭발
    AudioSource audioDestroyEnemy; // 적 기체 폭발
    AudioSource audioDestroyPlayer;// 아군 기체 폭발
    AudioSource audioDestroyAstroid;// 운석폭발
    AudioSource audioGetBomb;// 폭탄 획득
    AudioSource audioGetBonusScore;// 보너스 점수 획득
    AudioSource audioBeamCollision;// 빔 충돌
    void SetUp()
    {

        try
        {
            port = new SerialPort();
            port.PortName = "COM3";
            port.BaudRate = 9600;
            port.Parity = Parity.None;
            port.DataBits = 8;           
            port.Open();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
        

        table = TableManager.LoadTable<OptionTable>("optionTable");
        audioBeamFire = gameObject.AddComponent<AudioSource>();
        audioBeamFire.clip = Resources.Load<AudioClip>("Audio/weapon_player");

        audioBombShoot = gameObject.AddComponent<AudioSource>();
        audioBombShoot.clip = Resources.Load<AudioClip>("Audio/bomb_shoot");

        audioExplosionBomb = gameObject.AddComponent<AudioSource>();
        audioExplosionBomb.clip = Resources.Load<AudioClip>("Audio/bomb_explosion");

        audioDestroyEnemy = gameObject.AddComponent<AudioSource>();
        audioDestroyEnemy.clip = Resources.Load<AudioClip>("Audio/explosion_enemy");

        audioDestroyPlayer = gameObject.AddComponent<AudioSource>();
        audioDestroyPlayer.clip = Resources.Load<AudioClip>("Audio/explosion_player");

        audioDestroyAstroid = gameObject.AddComponent<AudioSource>();
        audioDestroyAstroid.clip = Resources.Load<AudioClip>("Audio/explosion_asteroid");

        audioGetBomb = gameObject.AddComponent<AudioSource>();
        audioGetBomb.clip = Resources.Load<AudioClip>("Audio/item_get");

        audioGetBonusScore = gameObject.AddComponent<AudioSource>();
        audioGetBonusScore.clip = Resources.Load<AudioClip>("Audio/get_bonus_score");

        audioBeamCollision = gameObject.AddComponent<AudioSource>();
        audioBeamCollision.clip = Resources.Load<AudioClip>("Audio/beam_collision");



    }

    private void OnApplicationQuit()
    {
        if (port.IsOpen)
            port.Close();
    }
    // Use this for initialization
    void Start ()
    {
        SetUp(); 
	}	
	// Update is called once per frame
	void Update ()
    {		
	}
}
