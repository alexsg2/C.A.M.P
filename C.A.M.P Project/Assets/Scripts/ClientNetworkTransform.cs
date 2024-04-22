using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode.Components;
using Unity.Netcode;

public class ClientNetworkTransform : NetworkTransform
{
    // client determines transform state
    protected override bool OnIsServerAuthoritative() {
        return false;
    }

    public override void OnNetworkSpawn() {
        base.OnNetworkSpawn();
        CanCommitToTransform = IsOwner;
    }

    protected override void Update() {
        base.Update();

        // TODO: check this!! may update ownership of transform to different clients
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsConnectedClient) {
            CanCommitToTransform = IsOwner;

            if (CanCommitToTransform) {
                TryCommitTransformToServer(transform, NetworkManager.LocalTime.Time);
            }
        }
    }
}
