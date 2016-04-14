using System.Collections.Generic;

public class TaskEnumeratorPlicator<T> : TaskSourcePlicator<T>
{
    protected virtual IEnumerator<T> Source { get; }

    public TaskEnumeratorPlicator(IEnumerator<T> source)
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
