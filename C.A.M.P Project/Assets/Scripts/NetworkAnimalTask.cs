using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public enum AnimalType {
    Bird,
    Land
}

public enum AnimalTaskStatus {
    Wait,
    AnimalsDone,
    Done
}

public class NetworkAnimalTask : NetworkBehaviour
{
    [SerializeField]
    private const int num_birds_needed = 1;

    [SerializeField]
    private const int num_land_animals_needed = 1;

    public NetworkVariable<int> land_animal_count = new NetworkVariable<int>(
        0, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);
    public NetworkVariable<int> bird_count = new NetworkVariable<int>(
        0, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);

    public NetworkVariable<AnimalTaskStatus> taskStatus = new NetworkVariable<AnimalTaskStatus>(
        AnimalTaskStatus.Wait, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);

    // Update is called once per frame
    void Update()
    {
        
    }
    

    // function called by server to update animal count when
    public void ServerIncrementAnimalCount(AnimalType type) {
        if (IsClient) {
            return;
        }

        switch (type) {
            case AnimalType.Bird:
                
                break;
            case AnimalType.Land:
                break;
        }
    }
}
