using System;
using UnityEngine;
using Steamworks;
using System.Threading.Tasks;
using Steamworks.Data;
using UnityEditor;
using System.Collections.Generic;

public class NetworkSystem : MonoBehaviour
{
    public bool Connected = false;
    public static NetworkSystem instance;
    public GameServer server;
    public GameClient client;
    //True if current instance is server
    public bool IsServer = true;
    //Network Player Prefab
    public GameObject PlayerInstance;
    //All player list
    public List<NetworkPlayerObject> PlayerList = new List<NetworkPlayerObject>();
    //Current Lobby player is in, no matter server or client
    public Lobby CurrentLobby;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Instance Already Exist");
            Destroy(this.gameObject);
        }

    }
    public NetworkPlayerObject SpawnPlayer(bool isLocal, int networkid, ulong steamid)
    {
        Debug.Log("Spawning Player");
        NetworkPlayerObject p = Instantiate(PlayerInstance, Vector3.zero, Quaternion.identity).GetComponent<NetworkPlayerObject>();
        p.NetworkID = networkid;
        p.steamID = steamid;
        p.IsLocal = isLocal;
        PlayerList.Add(p);
        return p;
    }
    public void RemoveAllPlayerObject()
    {
        foreach (NetworkPlayerObject g in PlayerList)
        {
            Destroy(g.gameObject);
        }
    }
    private void Start()
    {
        

        DontDestroyOnLoad(gameObject);
        try
        {
            SteamClient.Init(480, true);
            SteamNetworkingUtils.InitRelayNetworkAccess();

        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
#if UNITY_EDITOR

        EditorApplication.playModeStateChanged += OnExit;
#endif
        SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
        SteamMatchmaking.OnLobbyGameCreated += OnLobbyGameCreated;
        SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
        SteamFriends.OnGameLobbyJoinRequested += OnFriendJoinLobby;

    }
    public async void CreateGameLobby()
    {
        bool success = await CreateLobby();
        if (!success)
        {
            Debug.Log("Create Lobby Failed");
        }
    }
    private void NewLobby()
    {
        RemoveAllPlayerObject();
    }
#if UNITY_EDITOR
    private void OnExit(PlayModeStateChange change)
    {
        if (change == PlayModeStateChange.ExitingPlayMode)
        {
            OnDestroy();
        }
    }
#endif
    private void OnApplicationQuit()
    {
        OnDestroy();
    }


    private void OnDestroy()
    {

        if (server != null)
        {
            Debug.Log("Destroyed Server");
            server.DisconnectAll();
            server = null;
        }
        if (client != null)
        {
            client.Close();
            client = null;
        }
        SteamClient.Shutdown();
    }
    public async Task<bool> CreateLobby()
    {
        NewLobby();
        IsServer = true;
        client = null;
        try
        {
            var createLobbyOutput = await SteamMatchmaking.CreateLobbyAsync(8);
            if (!createLobbyOutput.HasValue)
            {
                Debug.Log("Lobby created but not correctly instantiated");
                throw new Exception();
            }

            Debug.Log("Successfully Created Lobby");

            return true;
        }
        catch (Exception exception)
        {
            Debug.Log("Failed to create multiplayer lobby");
            Debug.Log(exception.ToString());
            return false;
        }
    }
    private void Update()
    {
        if (server != null)
        {
            server.Receive();
        }
        if (client != null)
        {
            client.Receive();
        }
    }

    private async void OnLobbyCreated(Result r, Lobby l)
    {
        l.SetFriendsOnly();
        l.SetJoinable(true);
        l.Owner = new Friend(SteamClient.SteamId);
        Debug.Log($"Lobby ID: {l} Result: {r} Starting Game Server...");
        l.SetGameServer(SteamClient.SteamId);
        CurrentLobby = l;
        while(server == null)
        {
            try
            {
                server = SteamNetworkingSockets.CreateRelaySocket<GameServer>(1111);
                Debug.Log($"Successfully created Game Server");

            }
            catch (Exception ex)
            {
                Debug.LogError($"Please Restart your game Client | Error: {ex}");
                await Task.Delay(5000);

            }
        }
        
        await Task.Delay(300);
        Debug.Log($"Server: {server}");


        //Create the local Server Player

    }
    private void OnLobbyGameCreated(Lobby lobby, uint ip, ushort port, SteamId id)
    {
        if (id == SteamClient.SteamId) return;
        Debug.Log($"Connecting To Relay Server: {ip}:{port}, {id}");
        if (client == null)
        {
            IsServer = false;

            client = SteamNetworkingSockets.ConnectRelay<GameClient>(id);
            CurrentLobby = lobby;
        }
    }
    private void OnLobbyEntered(Lobby l)
    {
        if (l.Owner.Id == SteamClient.SteamId) { return; }
        NewLobby();

        if (client == null)
        {

            SteamId serverid = new SteamId();
            uint ip = 0;
            ushort port = 0;
            bool haveserver = l.GetGameServer(ref ip, ref port, ref serverid);
            if (haveserver)
            {
                Debug.Log($"Connecting To Relay Server: {ip}:{port}, {serverid}");
                CurrentLobby = l;
                server = null;
                IsServer = false;
                client = SteamNetworkingSockets.ConnectRelay<GameClient>(serverid, 1111);

            }
            else
            {
                Debug.Log($"No Server: {ip}:{port}, {serverid}");

            }



        }
    }
    private async void OnFriendJoinLobby(Lobby lobby, SteamId id)
    {
        if (server != null)
        {
            server.Close();
        }
        RoomEnter result = await lobby.Join();

        if (result != RoomEnter.Success)
        {
            Debug.Log($"Failed To Join Lobby created by {(new Friend(id)).Name}");
        }
        else
        {
            Debug.Log($"Joined Lobby created by {(new Friend(id)).Name}");

        }

    }
}
