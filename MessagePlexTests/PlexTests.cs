using System;
using System.Linq;
using System.Threading;
using Xunit;

public class PlexTests
{
    class MyChain : ConcurrentMessagePlicator<byte[]>
    {
        public bool IgnoreBreaking;

        public override bool Enlink(byte[] value)
            => Enlink(value, IgnoreBreaking);
    }

    [Fact]
    public void ForStream()
    {
        var buffer = new byte[3];

        var chain = new MyChain();
        var s0 = chain.ToPlexStream();
        var s1 = s0.Clone();
        chain.Enlink(new byte[] { 233 });
        var s2 = chain.ToPlexStream();
        chain.Enlink(new byte[] { 234 });

        using (s0)
        using (s1)
        using (s2)
        {
            new Thread(() =>
            {
                Thread.Sleep(1000);
                chain.Break();
                chain.Enlink(new byte[] { 255 });
            }).Start();

            Assert.Equal(233, s0.ReadByte());
            using (var s3 = s2.Clone())
            {
                Assert.Equal(234, s3.ReadByte());
                Assert.Equal(-1, s3.ReadByte());
                Assert.Equal(-1, s3.ReadByte());
            }


            Assert.Equal(1, s0.Read(buffer, 0, buffer.Length));
            Assert.Equal(234, buffer[0]);
            Assert.Equal(-1, s0.ReadByte());
            Assert.Equal(-1, s0.ReadByte());

            Assert.Equal(1, s1.Read(buffer, 0, buffer.Length));
            Assert.Equal(233, buffer[0]);
            Assert.Equal(1, s1.Read(buffer, 0, buffer.Length));
            Assert.Equal(234, buffer[0]);
            Assert.Equal(-1, s1.ReadByte());
            Assert.Equal(-1, s1.ReadByte());

            Assert.Equal(1, s2.Read(buffer, 0, buffer.Length));
            Assert.Equal(234, buffer[0]);
            Assert.Equal(-1, s2.ReadByte());
            Assert.Equal(-1, s2.ReadByte());
        }

        using (var s = chain.ToPlexStream())
        {
            Assert.Equal(-1, s.ReadByte());
            Assert.Equal(-1, s.ReadByte());
        }

        chain.IgnoreBreaking = true;
        chain.Enlink(null);

        using (var s = chain.ToPlexStream())
        {
            new Thread(() =>
            {
                Thread.Sleep(1000);
                chain.Enlink(new byte[] { 233, 234 });
                chain.Enlink(new byte[] { 253 });
                Thread.Sleep(1000);
                chain.Break();
            }).Start();

            Assert.Equal(2, s.Read(buffer, 0, buffer.Length));
            Assert.Equal(233, buffer[0]);
            Assert.Equal(234, buffer[1]);
            Assert.Equal(253, s.ReadByte());

            Assert.Equal(-1, s.ReadByte());
            Assert.Equal(-1, s.ReadByte());
        }
    }
}
