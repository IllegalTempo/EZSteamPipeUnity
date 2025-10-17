# EZ Steam PIPE
## BEFORE ALL
[Install Facepunch.Steamworks](https://github.com/Facepunch/Facepunch.Steamworks.git) <br>
This project relies on facepunch.steamworks
### About
This tool aims to help unity user to quickly import Multiplayer into their game. This tool provide custom packet management and basic network player script
### Hyper-Quick Start
1. Put scripts into your unity project
2. Put Editor folder
3. create/use a GameObject that is used for system script, add <**NetworkSystem**> script.
4. Add <**NetworkPlayerObject**> script to your Player GameObject
5. Prefab your Player GameObject ([Learn How - Unity Documentation](https://docs.unity3d.com/6000.2/Documentation/Manual/Prefabs.html))
6. Reference the Player prefab to **PlayerInstance** field in <**NetworkSystem**>
7. You can change any settings in <**NetworkSystem**>
8. Play!
### Proper Start
#### - Routine for adding packets:
1. Unity Editor -> Tools -> Packet Manager
2. Here you can view all the packet relationship and create packets
3. follow the format of other packets in <**PacketHandles**> and <**PacketSend**> for handling packet data and sending data
4. For handling packet::
    1. Read data from the packet object with **SAME** order as how you write them
    2. Have fun with these data!
5. For Sending packets:
    1. Put arguments in the method as you want
    2. packet.write(your variables)
    3. - Broadcast/NetworkPlayer.SendPacket with Server packets
       - SendToServer with Client packets
As server, 
- you can access specific player NetworkPlayerObject using GameServer.players[ player steam id ].player
- Get player steamid by using a NetworkID (Host ID is always 0)
As client,
- Get NetworkPlayerObject using a NetworkID
### NetworkPlayerObject vs NetworkPlayer
- NetworkPlayerObject stores and do more things about unity and gameObjects
- NetworkPlayer do more about Networking, SendPacket method will directly send packet to target player (haven't tried it client -> client)
### More Tips
- use IsLocal in NetworkPlayerObject to identify whether that GameObject is owned by the client, you may use it in movements eg do not move responding to input if not local (Not controlled by player)