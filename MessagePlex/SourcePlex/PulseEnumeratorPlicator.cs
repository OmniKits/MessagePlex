using System.Collections.Generic;

namespace MessagePlex
{
    public class PulseEnumeratorPlicator<T> : PulseSourcePlicator<T>
    {
        protected virtual IEnumerator<T> Source { get; }

        public PulseEnumeratorPlicator(IEnumerator<T> source)
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
}
