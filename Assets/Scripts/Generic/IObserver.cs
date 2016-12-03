public enum messages { SQUAD_DEAD }

public interface IAlienObserver
{
    void notify(messages msg);
}

public interface IAlienObservable
{
    void subscribeNotifier(IAlienObserver observer);
    void unsubscribeNotifier(IAlienObserver observer);
}

