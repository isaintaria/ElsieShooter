using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Collections.Generic;

public class Done_GameController : MonoBehaviour
{
    
	public GameObject[] hazards;
    public GameObject[] backgroundHazards;
    public GameObject playerObject;
    public GameObject playerSpawnPosition;
    public GameObject itemObject;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
    public int playTime;

    private int deadCount = 0;

    public AudioSource soundItemGet;
    
	
	public GUIText scoreText;
	public GUIText timeText;
	public GUIText gameOverText;
    public GUIText bombMaxText;
    public GUIText finalScoreText;
    public GUIText currentBombText;
    

    public int bombMaxCount;
    public int startBombCount;
    
	
	private bool gameOver;
	private bool restart;
    private bool isPlayerDead;
	private int score;
    public static  bool timeOver;
    private GameObject player;

    private static float speedWeight = 1.0f;

    private int bombCount;
    private int time;

    public static bool EnabledVisualMode = false;
    public static bool EnabledSoundMode = false;
    public static bool EnabledHapticMode = false;
    public static bool Enabled3DMode = false;

    public GameObject Camera2D;
    public GameObject Camera3D;


    TutorialInfo infoMenu;
       
    public void SetOption()
    {
        SetCameraMode(Enabled3DMode);
    }

    public void SetCameraMode(bool mode)
    {
        if (mode)
        {
            Camera2D.SetActive(false);
            Camera3D.SetActive(true);
        }
        else
        {
            Camera2D.SetActive(true);
            Camera3D.SetActive(false);
        }
    }
    public int BombCount
    {
        get
        {
            return bombCount;
        }

        set
        {
            bombCount = value;
            if (bombCount >= bombMaxCount)
                bombCount = bombMaxCount;
            currentBombText.text = bombCount.ToString();
        }
    }

    public bool IsPlayerDead
    {
        get
        {
            return isPlayerDead;
        }

        set
        {
            isPlayerDead = value;
        }
    }

    public static float SpeedWeight
    {
        get
        {
            return speedWeight;
        }

        set
        {
            speedWeight = value;
        }
    }

    private LevelTable levelTable; 
    void Start ()
	{
        infoMenu = GameObject.Find("Info Menu").GetComponent<TutorialInfo>();
        SetOption();
        timeOver = true;
        startWait = 1;
        levelTable = TableManager.LoadTable<LevelTable>("LevelTable");        
        time = playTime;
        isPlayerDead = false;        
		gameOver = false;
		restart = false;
		timeText.text = "";
		gameOverText.text = "";
        finalScoreText.text = "";
        bombCount = startBombCount;       
		score = 0;
		UpdateScore ();
        bombMaxText.text = bombMaxCount.ToString();
        currentBombText.text = bombCount.ToString();
		StartCoroutine (SpawnWaves ());
        StartCoroutine(SpawnBackgroundLeft());
        StartCoroutine(SpawnBackgroundRight());
        StartCoroutine( TimerRoutine() );
        respawnPlayer();
        
	}

    private IEnumerator SpawnBackgroundRight()
    {
        while (true)
        {
            GameObject hazard = backgroundHazards[UnityEngine.Random.Range(0, 3)];            
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(spawnValues.x+2f, spawnValues.x+10f), spawnValues.y, spawnValues.z);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(hazard, spawnPosition, spawnRotation);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator SpawnBackgroundLeft()
    {
        while(true)
        {
            GameObject hazard = backgroundHazards[UnityEngine.Random.Range(0, 3)];            
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-spawnValues.x - 10f, -spawnValues.x-2f), spawnValues.y, spawnValues.z);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(hazard, spawnPosition, spawnRotation);
            yield return new WaitForSeconds(0.1f);
        }        
    }

    private IEnumerator TimerRoutine()
    {
        while(time > 0)
        {
            time--;
            var span = new TimeSpan(0, 0, time);
            DateTime dt = new DateTime(span.Ticks);
            timeText.text = "남은 시간 : " + dt.ToString("mm:ss");
            yield return new WaitForSeconds(1);
        }
        finalScoreText.text = score + " 점";
        gameOverText.text = "GAME CLEAR \n연구원을 호출 해 주세요";
        timeOver = true;
        
    }

    void Update ()
	{
        if(timeOver)
        {
            Time.timeScale = 0f;            
            if (Input.GetKeyDown(KeyCode.R))
            {                
                timeOver = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else
            Time.timeScale = 1f;
        //if (restart)
        //{
        //	if (Input.GetKeyDown (KeyCode.R))
        //	{
        //              SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //	}
        //}


    }

    void Shuffle(ref List<int> list)
    {
        int n = list.Count;
        System.Random rnd = new System.Random();
        while (n > 1)
        {
            int k = (rnd.Next(0, n) % n);
            n--;
            int value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		for(int i = 0; i < levelTable.Datas.Length; i++)
		{
            Debug.Log("웨이브 시작");
            var ds = levelTable.Datas[i];
            SpeedWeight = ds.speed; // 스피드 업
            // 레벨테이블의 아이템의 전체 수가 한번의 사이클
            // spawnWait은 15 나누기 해당 레벨의 모든 적기의 숫자로 정의한다
            List<int> hList = SetHazardWave(ds);             
            for (int j = 0; j < hList.Count; j++)
			{
                //GameObject hazard = hazards [UnityEngine.Random.Range (0, hazards.Length)];
                GameObject hazard = hazards[hList[j]];                
                Vector3 spawnPosition = new Vector3 (UnityEngine.Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (15.0f/hList.Count);
			}
            if( timeOver )
            {
                Debug.Log("Time is Over");
            }
            Debug.Log("웨이브 종료");
            yield return new WaitForSeconds(waveWait);
        }

        Debug.Log("게임 종료됨");
	}

    private List<int> SetHazardWave(LevelProperty ds)
    {
        List<int> retList = new List<int>();
        for (int i = 0; i < ds.ast1; i++)
            retList.Add(0);
        for (int i = 0; i < ds.ast2; i++)
            retList.Add(1);
        for (int i = 0; i < ds.ast3; i++)
            retList.Add(2);
        for (int i = 0; i < ds.normal; i++)
            retList.Add(3);
        for (int i = 0; i < ds.green; i++)
            retList.Add(4);
        for (int i = 0; i < ds.red; i++)
            retList.Add(5);

        Shuffle(ref retList);

        return retList;
    }

    public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
        if (score < 0)
            score = 0;
		UpdateScore ();
	}
	
	void UpdateScore ()
	{
		scoreText.text = "Score: " + score;
	}
	
	public void GameOver ()
	{
		gameOverText.text = "Game Over!";
		gameOver = true;
	}

    public void PlayerDead()
    {
        if( !isPlayerDead )
        {
            deadCount++;
            if (deadCount > 5)
                deadCount = 5;
            isPlayerDead = true;
            respawnPlayer();
            AddScore(-100 * deadCount);
            
        }
          
    }

    private void respawnPlayer()
    {       
        StartCoroutine(respawnAction());        
        isPlayerDead = false;
    }

    //3초 뒤에 부활 하는 걸루 하자.
    private IEnumerator respawnAction()
    {
        yield return new WaitForSeconds(startWait);

        var obj = Instantiate(playerObject, playerSpawnPosition.transform.position, playerSpawnPosition.transform.rotation);
        var renderer = obj.GetComponent<MeshRenderer>();
        var collider = obj.GetComponent<BoxCollider>();
        var controller = obj.GetComponent<Done_PlayerController>();       
        var rigidbody = obj.GetComponent<Rigidbody>();


        collider.enabled = false;

        // 리스폰 애니메이션 구현
        while (obj.transform.position.z < -2)
        {
            controller.IsMoveable = false;
            controller.MoveUp();
            yield return new WaitForSeconds(0);
        }
        controller.IsMoveable = true;
             

        for( int i = 0; i < 5; i++)
        {
            obj.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            obj.SetActive(true);
            yield return new WaitForSeconds(0.3f);
        }


        collider.enabled = true;




    }
}