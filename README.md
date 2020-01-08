# Pacmen

What is Pacmen? No, it is not simply a mispronounced Pacman clone.
It is a multiplayer Pacman clone!

This project is being developed for the 2017/18
**Design and Implementation of Distributed Applications** course at **Instituto Superior Técnico**.

## Project Description

The goal of this project is to develop a fault-tolerant multiplayer .NET application named Pacmen.
This aims at creating a server-client resilient to crashes and network delays through server replication.

For more detailed description check the document DAD-Project-1718.pdf
Project report at Report/latex8.pdf

## Baseline Functionality
Users can:
1. connect to a well-known centralized server to play a pacman game;
2. communicate with other players in a P2P fashion, where the messages respect a **causal order**.

## Passive Replication
When server is started:

 - Tell it if it's primary or secondary
     - If secondary, also pass it the primary server address
 - Secondary server requests the primary server's current state (including list of secondary servers)
 - Assign a deterministic ordering of replicas
 - Primary server sends heartbeats to secondary servers
 - Whenever there's a state update, replicas are also updated
 - After some timeout, the next replica in the ordered list takes over
     - Use consensus to determine the new "leader", to prevent wrong takeovers

## Client side concerns
 - Define method for changing the active server
     - This method is called by a replica when it takes over
 - Implement a prediction mechanism for the next state
      - If the client doesn't hear back from the server after the round time, it calculates the next state from the information it currently holds
      - When the server reconnects to the client, it updates the state with the "real" state from the server

## Client fault tolerance
When a client is assumed to have crashed:

  - Last input received is stored and server assumes it is holding the key.
  - When it reconnects, it receives the updated state and renders it.