using System;
using System.Collections.Generic;

public interface IDoppelGanger<T> : IEnumerator<T>
#if NETFX
    , ICloneable
#endif
{
    new IDoppelGanger<T> Clone();
}
