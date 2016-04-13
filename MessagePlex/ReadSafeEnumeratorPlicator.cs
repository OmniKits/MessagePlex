using System.Collections.Generic;

public class ReadSafeEnumeratorPlicator<T> : ReadSafeSourcePlicator<T>
{
    protected virtual IEnumerator<T> Source { get; }

    public ReadSafeEnumeratorPlicator(IEnumerator<T> source)
    {
        Source = source;
    }

    protected sealed override bool TryReadMessage(out T result)
    {
        var success = Source.MoveNext();
        result = success ? Source.Current : default(T);
        return success;
    }
}
