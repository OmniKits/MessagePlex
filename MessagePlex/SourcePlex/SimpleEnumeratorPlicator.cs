using System.Collections.Generic;

public class SimpleEnumeratorPlicator<T> : SimpleSourcePlicator<T>
{
    protected virtual IEnumerator<T> Source { get; }

    public SimpleEnumeratorPlicator(IEnumerator<T> source)
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
