using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

using MessagePlex;

[Collection(nameof(MemoryLeakingTests))]
public class MemoryLeakingTests
{
    [CollectionDefinition(nameof(MemoryLeakingTests))]
    [Orderer(Order = 1)]
    public class Fixture : ICollectionFixture<MemoryLeakingTests>
    { }

    private static readonly int BASE;

    static MemoryLeakingTests()
    {
        BASE = Environment.Is64BitProcess ? 0x40000 : 0x10000;
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
                if (test != null)
                    Assert.True(test(data, i++));

                if (mode.Has(TestMode.CoMoveNext))
                    ator.MoveNext();
            }

            if (ator.MoveNext() && test != null)
                Assert.True(test(ator.Current, 0));
        }
    }

    public void ForModes<T>(Func<IMessagePlicator<T>> factory, Func<T, int, bool> test)
    {
        try
        {
            TestPlicator(factory());
            throw new Exception();
        }
        catch (Exception ex)
        {
            if (ex is AggregateException)
                ex = ex.InnerException;
            Assert.IsType<OutOfMemoryException>(ex);
        }

        TestPlicator(factory(), test, TestMode.CoMoveNext);
        TestPlicator(factory(), test, TestMode.CoClose);
    }

    private IEnumerator<byte[]> GenEnumerator()
    {
        int i;
        for (i = 0; i < BASE; i++)
            yield return new byte[BASE + i];
    }

    [Fact]
    [Orderer(Order = -1)]
    public void ForBlockingEnumeratorPlicator()
        => ForModes(() => new BlockingEnumeratorPlicator<byte[]>(GenEnumerator()), (data, i) => data.Length == BASE + i);

    [Fact]
    [Orderer(Order = -2)]
    public void ForNaiveEnumeratorPlicator()
        => ForModes(() => new NaiveEnumeratorPlicator<byte[]>(GenEnumerator()), (data, i) => data.Length == BASE + i);
}
