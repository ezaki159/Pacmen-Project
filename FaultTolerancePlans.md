# Passive Replication
When server is started:

 - Tell it if it's primary or secondary
     - If secondary, also pass it the primary server address
 - Secondary server requests the primary server's current state
   (including list of secondary servers)
 - Assign a deterministic ordering of replicas
 - Primary server sends heartbeats to secondary servers
 - Whenever there's a state update, replicas are also updated
 - After some timeout, the next replica in the ordered list
   takes over
     - Use consensus to determine the new "leader", to prevent
       wrong takeovers

# Client side concerns
 - Define method for changing the active server
     - This method is called by a replica when it takes over
 - Implement a prediction mechanism for the next state
      - If the client doesn't hear back from the server after
        the round time, it calculates the next state from the
        information it currently holds
      - When the server reconnects to the client, it updates
        the state with the "real" state from the server

# Client fault tolerance
When a client is assumed to have crashed:

  - Last input received is stored and server assumes it is
    holding the key.
  - When it reconnects, it receives the updated state and
    renders it.
