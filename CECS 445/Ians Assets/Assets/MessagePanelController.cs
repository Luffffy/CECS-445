using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagePanelController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = this.transform.parent.transform.position;
        Debug.Log(this.transform.parent.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
