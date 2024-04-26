using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAcceptor : MonoBehaviour
{
    public CapsuleCollider coll; 
    public GameObject to_accept; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "QuestItem") {
            Debug.Log("QuestItem Accepted");
            other.gameObject.SetActive(false);
        }

    }
}
