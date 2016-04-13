using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

public class MemoryLeakingTests
{
    public MemoryLeakingTests()
    {
        Thread.Sleep(TimeSpan.FromSeconds(5));
    }

    [Flags]
    enum TestMode
    {
        Default = 0,
        CoMoveNext = 1,
        CoClose = 2,
    }

    void TestPlicator<T>(IMessagePlicator<T> plicator, Func<T, int, bool> test = null, TestMode mode = TestMode.Default)
    {
        using (plicator)
        {
            var ator = plicator.GetEnumerator();
            if (mode.Has(TestMode.CoClose))
                ator.Dispose();

            int i = 0;
            foreach (var data in plicator)
            {
                if (mode.Has(TestMode.CoMoveNext))
                    ator.MoveNext();

                if (test != null)
                    Assert.True(test(data, i++));
            }
        }
    }

    public void ForModes<T>(Func<IMessagePlicator<T>> factory, Func<T, int, bool> test)
    {
        try
        {
            TestPlicator(factory());
            throw new Exception();
        }
        catch (AggregateException ex)
        {
            Assert.IsType<OutOfMemoryException>(ex.InnerException);
        }

        TestPlicator(factory(), test, TestMode.CoMoveNext);
        TestPlicator(factory(), test, TestMode.CoClose);
    }

    private IEnumerator<byte[]> GenEnumerator()
    {
        for (var i = 0; i < 0x10000; i++)
            yield return new byte[0x10000 + i];
    }

    [Fact]
    public void ForReadSafeEnumeratorPlicator()
        => ForModes(() => new ReadSafeEnumeratorPlicator<byte[]>(GenEnumerator()), (data, i) => data.Length == 0x10000 + i);

    [Fact]
    public void ForSimpleEnumeratorPlicator()
        => ForModes(() => new SimpleEnumeratorPlicator<byte[]>(GenEnumerator()), (data, i) => data.Length == 0x10000 + i);
}
