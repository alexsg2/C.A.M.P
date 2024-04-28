using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public enum AnimalType {
    Land,
    Bird
}

public enum AnimalTaskStatus {
    Wait,
    Start,
    Done // when clicked enough land animals and birds
}

/// <summary>
/// Client-driven class for animal task. On StartTask() call,
/// clients register listeners for all animals OnAnimalClick delegates.
/// Client callback sends RPC to server to increment count of animal type.
/// </summary>
public class NetworkAnimalTask : NetworkBehaviour
{
    [SerializeField]
    private const int num_birds_needed = 1;

    [SerializeField]
    private const int num_land_animals_needed = 1;

    public List<AnimalWiki> land_animals;
    public List<AnimalWiki> birds;

    // private NetworkVariable<int> land_animal_count = new NetworkVariable<int>(
    //     0, 
    //     NetworkVariableReadPermission.Everyone, 
    //     NetworkVariableWritePermission.Server);
    // private NetworkVariable<int> bird_count = new NetworkVariable<int>(
    //     0, 
    //     NetworkVariableReadPermission.Everyone, 
    //     NetworkVariableWritePermission.Server);

    // *should* only be written to by server
    private int land_animal_count = 0;
    private int bird_count = 0;

    // left = land animals task, right = birds task
    public  NetworkVariable<TwoBools> taskStatus = new NetworkVariable<TwoBools>(
        default, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server);

    // Make clients start listening for clicks on animals.
    public void StartTask() {
        if (IsServer) {
            return;
        }
        // Debug.Log("NetworkAnimalTask: started!");

        // register callbacks only for clients
        foreach (AnimalWiki land_animal in land_animals) {
            land_animal.OnAnimalClick += OnClientClickAnimal;
        }

        foreach (AnimalWiki bird in birds) {
            bird.OnAnimalClick += OnClientClickAnimal;
        }

        taskStatus.OnValueChanged += OnTaskStatusChange;
    }

    // Deregister listeners. Should only be executed by clients.
    private void OnTaskStatusChange(TwoBools old, TwoBools updated) {
        if (IsServer) {
            return;
        }

        // if we didnt already deregister listeners for
        // land animal task, do so now
        if (!old.left && updated.left) {
            // Debug.Log("NetworkAnimalTask: land animal subtask completed!");
            foreach (AnimalWiki wiki in land_animals) {
                wiki.OnAnimalClick -= OnClientClickAnimal;
            }
        }

        // same here for bird task
        if (!old.right && updated.right) {
            // Debug.Log("NetworkAnimalTask: bird subtask completed!");
            foreach (AnimalWiki wiki in birds) {
                wiki.OnAnimalClick -= OnClientClickAnimal;
            }
        }

        if (updated.left && updated.right) {
            // Debug.Log("NetworkAnimalTask: both subtasks completed!");
            taskStatus.OnValueChanged -= OnTaskStatusChange;
        }
    }

    /// <summary>
    /// Executed by clients as a callback from AnimalWiki when
    /// they click an animal.
    /// </summary>
    /// <param name="wiki"></param>
    /// <param name="type"></param>
    public void OnClientClickAnimal(AnimalWiki wiki, AnimalType type) {
        if (IsServer) {
            return;
        }
        // Debug.Log($"NetworkAnimalTask: client {OwnerClientId} clicked a {type} animal. Sending RPC.");

        IncrementAnimalCountServerRpc(type);

        // deregister this animal listener, can only click once
        wiki.OnAnimalClick -= this.OnClientClickAnimal;
    }

        
    [ServerRpc(RequireOwnership = false)]
    public void IncrementAnimalCountServerRpc(AnimalType type) {
        // Debug.Log("NetworkAnimalTask: Server received animal rpc");
        TwoBools task_stat = taskStatus.Value;

        if (!task_stat.left && type == AnimalType.Land) {
           
            land_animal_count++;
            // Debug.Log($"NetworkAnimalTask: Server updated land animal count to {land_animal_count}");
            // update task status if needed
            if (land_animal_count >= num_land_animals_needed) {
                task_stat.left = true;
                taskStatus.Value = task_stat;
            }
            return;
        }

        if (!task_stat.right && type == AnimalType.Bird) {
            bird_count++;
            // Debug.Log($"NetworkAnimalTask: Server updated bird count to {bird_count}");
            // update task status if needed
            if(bird_count >= num_birds_needed) {
                task_stat.right = true;
                taskStatus.Value = task_stat;
            }
            return;
        }
    }
}
