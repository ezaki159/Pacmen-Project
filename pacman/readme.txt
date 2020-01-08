All mentioned *.exe files, with the exception of PuppetMaster.exe, will be located
in the ProcessCreationService/bin/Release/ folder. PuppetMaster.exe is in the
PuppetMaster/bin/Release folder.

How to start:
 -A Primary Server:
   -Open the provided Server.exe.
   -On the right hand side, fill in the "Port" box with the desired port for the server to run.
   -Select Pacman in the "Game" dropdown box.
   -Input the desired number of players to wait at "# of players:"
   -Insert desired round time at "Input time".
   -Press "Start"
   -Done~
 -A Replica Server:
   -Open the provided Server.exe.
   -On the right hand side, fill in the "Port" box with the desired port for the server to run.
   -Select Pacman in the "Game" dropdown box.
   -Tick the checkbox labeled "Replica Server".
   -Fill in the primary server ip address / hostname at "Server address".
   -Input primary server port at "Port"
   -Press "Start"
   -Done~
 -A Client
   -Open the provided Client.exe.
   -On the right had side, fill in the game server ip address / hostname at "Server address" and port at "Port"
   -Input desired "Nickname" for the player, but be aware that not the inserted nickname might not be available (it uniquely identifies a player).
   -[Optional] if desired, a tracefile script can be used for player movements, and to do so, simply tick the checkbox named "Tracefile" and next input the relative filepath of the desired tracefile to use.
   -A "Connect" should be enable by now and simply press it to fire a client (the client will be waiting for the game server to start the game).
   -Play the game using the WASD keys for movement
 -A PCS
   -Simply execute ProcessCreationService.exe.
 -A PuppetMaster
   -Open the provided PuppetMaster.exe.
   -Input the desired commands using the assignment command format.
   -[Optional] In order to execute a plot file open a command line in the current folder, and execute PuppetMaster.exe [plotfilename].

Missing features:
-None~