using UnityEngine;

public interface Observer
{
    void ReceiveUpdate(Object observee, Message message);
}
