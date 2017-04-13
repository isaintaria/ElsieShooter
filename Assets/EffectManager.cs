using Assets._Completed_Assets.Scripts.TableManager.TableItems;
using UnityEngine;
using System.IO.Ports;
using System;
using System.Collections;

using System.Text;
using System.Net.Sockets;
using System.Net;

using System.Threading;


//// State object for receiving data from remote device.
//public class StateObject
//{
//    // Client socket.
//    public Socket workSocket = null;
//    // Size of receive buffer.
//    public const int BufferSize = 256;
//    // Receive buffer.
//    public byte[] buffer = new byte[BufferSize];
//    // Received data string.
//    public StringBuilder sb = new StringBuilder();
//}

//public class AsynchronousClient
//{
//    // The port number for the remote device.
//    private const int port = 11000;

//    // ManualResetEvent instances signal completion.
//    private  ManualResetEvent connectDone =
//        new ManualResetEvent(false);
//    private  ManualResetEvent sendDone =
//        new ManualResetEvent(false);
//    private  ManualResetEvent receiveDone =
//        new ManualResetEvent(false);

//    // The response from the remote device.
//    private  String response = String.Empty;

//    public void StartClient(string wavUrl)
//    {
//        // Connect to a remote device.
//        try
//        {
//            // Establish the remote endpoint for the socket.
//            // The name of the 
//            // remote device is "host.contoso.com".

//            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
//            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

//            // Create a TCP/IP socket.
//            Socket client = new Socket(AddressFamily.InterNetwork,
//                SocketType.Stream, ProtocolType.Tcp);

//            // Connect to the remote endpoint.
//            client.BeginConnect(remoteEP,
//                new AsyncCallback(ConnectCallback), client);
//            connectDone.WaitOne();

//             // Send test data to the remote device.
//            Send(client, wavUrl);
//            sendDone.WaitOne();
//            // Receive the response from the remote device.
//            Receive(client);
//            receiveDone.WaitOne();

//            // Write the response to the console.
//            Debug.Log(string.Format("Response received : {0}", response));

//            // Release the socket.  
//            client.Shutdown(SocketShutdown.Both);
//            client.Close();
      
   

//        }
//        catch (Exception e)
//        {
//            Debug.Log(e.ToString());
//        }
//    }

//    private  void ConnectCallback(IAsyncResult ar)
//    {
//        try
//        {
//            // Retrieve the socket from the state object.
//            Socket client = (Socket)ar.AsyncState;

//            // Complete the connection.
//            client.EndConnect(ar);

//            Debug.Log(string.Format("Socket connected to {0}",
//                client.RemoteEndPoint.ToString()));

//            // Signal that the connection has been made.
//            connectDone.Set();
//        }
//        catch (Exception e)
//        {
//            Debug.Log(e.ToString());
//        }
//    }

//    private  void Receive(Socket client)
//    {
//        try
//        {
//            // Create the state object.
//            StateObject state = new StateObject();
//            state.workSocket = client;

//            // Begin receiving the data from the remote device.
//            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
//                new AsyncCallback(ReceiveCallback), state);
//        }
//        catch (Exception e)
//        {
//            Debug.Log(e.ToString());
//        }
//    }

//    private  void ReceiveCallback(IAsyncResult ar)
//    {
//        try
//        {
//            // Retrieve the state object and the client socket 
//            // from the asynchronous state object.
//            StateObject state = (StateObject)ar.AsyncState;
//            Socket client = state.workSocket;

//            // Read data from the remote device.
//            int bytesRead = client.EndReceive(ar);

//            if (bytesRead > 0)
//            {
//                // There might be more data, so store the data received so far.
//                state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));

//                // Get the rest of the data.
//                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
//                    new AsyncCallback(ReceiveCallback), state);
//            }
//            else
//            {
//                // All the data has arrived; put it in response.
//                if (state.sb.Length > 1)
//                {
//                    response = state.sb.ToString();
//                }
//                // Signal that all bytes have been received.
//                receiveDone.Set();
//            }
//        }
//        catch (Exception e)
//        {
//            Debug.Log(e.ToString());
//        }
//    }

//    private  void Send(Socket client, String data)
//    {
//        // Convert the string data to byte data using ASCII encoding.
//        byte[] byteData = Encoding.UTF8.GetBytes(data);

//        // Begin sending the data to the remote device.
//        client.BeginSend(byteData, 0, byteData.Length, 0,
//            new AsyncCallback(SendCallback), client);
//    }

//    private  void SendCallback(IAsyncResult ar)
//    {
//        try
//        {
//            // Retrieve the socket from the state object.
//            Socket client = (Socket)ar.AsyncState;

//            // Complete sending the data to the remote device.
//            int bytesSent = client.EndSend(ar);
//            Debug.Log(string.Format("Sent {0} bytes to server.", bytesSent));

//            // Signal that all bytes have been sent.
//            sendDone.Set();
//        }
//        catch (Exception e)
//        {
//            Debug.Log(e.ToString());
//        }
//    }
//}


public class EffectManager : MonoBehaviour {

 
    OptionTable table;
    SerialPort bulbPort;
    SerialPort vibePort;


    
     
    private void MakeEffect(string loc,EffectOption effect,AudioSource source)
    {
        if (source != null)
            source.Play();
        if (effect.bulb && Done_GameController.EnabledVisualMode)
            EffectBulb(effect.pattern_b,loc);
        if (effect.vibration && Done_GameController.EnabledHapticMode)
            EffectVibration(effect.pattern_v,loc);
                                   
    }

    private void EffectSpeaker(string str = "Debug")
    {
        Debug.Log(str + "에서  소리 이펙트 발생");

        try
        {   
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private  void EffectVibration(int pattern_v,string str ="Debug")
    {
        Debug.Log(str + "에서  진동 이펙트 발생 패턴:" + pattern_v);
        StartCoroutine(VibeTest());
    }

    private  void EffectBulb(int pattern_b, string str = "Debug")
    {
        Debug.Log(str + "에서 전구 이펙트 발생 패턴:" +pattern_b); 
        StartCoroutine(BulbTest());
    }

    private IEnumerator BulbTest()
    {
        if( bulbPort.IsOpen )
        {
            // port2.Write(new char[] { '0' }, 0, 1);
            bulbPort.Write(new char[] { '0' }, 0, 1);
            yield return new WaitForSeconds(0.6f);
            bulbPort.Write(new char[] { '1' }, 0, 1);
        }
        else
        {

        }

    }

    private IEnumerator VibeTest()
    {
        if( bulbPort.IsOpen )
        {
            // port2.Write(new char[] { '0' }, 0, 1);
            bulbPort.Write(new char[] { '0' }, 0, 1);
            yield return new WaitForSeconds(0.6f);
            bulbPort.Write(new char[] { '1' }, 0, 1);
        }
        else
        {

        }
        
    }

    public void FirePlayerBeamEffect()
    {
        MakeEffect("weapon_player.wav", table.FireBeam, audioBeamFire);
    }
    public void FirePlayerBombExplosionEffect()
    {
        MakeEffect("bomb_explosion.wav", table.Explosion_Bomb, audioExplosionBomb);
    }
    public void FirePlayerBombShootEffect()
    {
        MakeEffect("bomb_shoot.wav", table.FireBomb, audioBombShoot);
    }
    public void FireEnemyDeadByPlayerBeamEffect()
    {
        MakeEffect("explosion_enemy.wav", table.Destroy_Enemy, audioDestroyEnemy);
    }
    public void FirePlayerDeadEffect()
    {
        MakeEffect("explosion_player.wav", table.Destroy_Player, audioDestroyPlayer);
    }
    public void FireAstroidExplosionEffect()
    {
        MakeEffect("explosion_asteroid.wav", table.Destroy_Astroid, audioDestroyAstroid);
    }
    public void FireGetBombEffect()
    {
        MakeEffect("item_get.wav", table.GetBomb, audioGetBomb);
    }
    public void FireGetBonusEffect()
    {
        MakeEffect("get_bonus_score.wav", table.GetBonusScore, audioGetBonusScore);
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
            var sTable = TableManager.Load<PortTable>("PortTable");
            bulbPort = new SerialPort();
            bulbPort.PortName = sTable.Bulb_Port.PORT;
            bulbPort.BaudRate = 9600;
            bulbPort.Parity = Parity.None;
            bulbPort.DataBits = 8;           
            bulbPort.Open();

            vibePort = new SerialPort();
            vibePort.PortName = sTable.Vibe_Port.PORT;
            vibePort.BaudRate = 9600;
            vibePort.Parity = Parity.None;
            vibePort.DataBits = 8;
            vibePort.Open();


        }
        catch (System.Exception e)
        {
            TutorialInfo InfoMenu = GameObject.Find("Info Menu").GetComponent<TutorialInfo>();
            InfoMenu.ShowErrorMessage("아두이노 연결에 문제가 있습니다. 외부 효과가 나타나지 않습니다. \n" +e.Message);
        }
        

        table = TableManager.Load<OptionTable>("optionTable");
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
        if (bulbPort.IsOpen)
        {
            bulbPort.Write(new char[] { '0' }, 0, 1);
            bulbPort.Close();
        }
        if( vibePort.IsOpen)
        {
            vibePort.Write(new char[] { '0' }, 0, 1);
            vibePort.Close();
        }
      
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
