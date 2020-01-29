using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Subject
{
    void AttachObserver(Observer o);
    void DettachObserver(Observer o);
    void NotifyObservers();
}
