using System;
using System.Collections.Generic;

public abstract class ReadSafeEnumeratorPlicator<T> : ReadSafeSourcePlicator<T>
{
    protected abstract IEnumerator<T> Source { get; }

    protected sealed override void FeedMe()
    {
        if (Source.MoveNext())
            Enlink(Source.Current, true);
        else
            Break();
    }
}
