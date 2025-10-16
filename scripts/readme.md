# EZ Steam PIPE
## BEFORE ALL
[Install Facepunch.Steamworks](https://github.com/Facepunch/Facepunch.Steamworks.git) <br>
This project relies on facepunch.steamworks
### About
This tool aims to help unity user to quickly import Multiplayer into their game. This tool provide custom packet management and basic network player script
### Hyper-Quick Start
1. Put scripts into your unity project
2. create/use a GameObject that is used for system script, add <**NetworkSystem**> script.
3. Add <**NetworkPlayerObject**> script to your Player GameObject
4. Prefab your Player GameObject ([Learn How - Unity Documentation](https://docs.unity3d.com/6000.2/Documentation/Manual/Prefabs.html))
5. Reference the Player prefab to **PlayerInstance** field in <**NetworkSystem**>
6. You can change any settings in <**NetworkSystem**>
7. Play!
### Proper Start
#### - Routine for adding packets:
1. 
