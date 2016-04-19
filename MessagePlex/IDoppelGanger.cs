using System;
using System.Collections.Generic;
using System.Text;

public interface IDoppelGanger<T> : IEnumerator<T>
#if NETFX
    , ICloneable
#endif
{
    new IDoppelGanger<T> Clone();
}
