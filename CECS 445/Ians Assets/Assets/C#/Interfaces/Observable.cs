
public interface Observable
{
    void AttachObserver(Observer o);
    void DettachObserver(Observer o);
    void NotifyObservers();
}
