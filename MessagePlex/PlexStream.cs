using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public sealed class PlexStream : Stream
#if NETFX
    , ICloneable
#endif
{
    private IPlexBeaconPin<byte[]> _ArrLink;

    private byte[] _Data;
    private int DataLength => (_Data?.Length).GetValueOrDefault();
    private int _Position;
    internal PlexStream(IPlexBeaconPin<byte[]> link, int position = 0)
    {
        _ArrLink = link;
        _Position = position;
    }
    private void Advance(IPlexBeaconPin<byte[]> link)
    {
        _ArrLink = link;
        _Data = _ArrLink?.Message;
        _Position = 0;
    }

    public override bool CanRead => true;
    private int ReadCore(byte[] buffer, int offset, int count)
    {
        Array.Copy(_Data, _Position, buffer, offset, count);
        _Position += count;
        return count;
    }
    public override int Read(byte[] buffer, int offset, int count)
    {
        var r = DataLength - _Position;
        if (r <= 0)
        {
            if (_ArrLink == null)
                return 0;

            Advance(_ArrLink.Next);
            return Read(buffer, offset, count);
        }

        return ReadCore(buffer, offset, r < count ? r : count);
    }
#if AWAIT
    public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken token = default(CancellationToken))
    {
        var r = DataLength - _Position;
        if (r <= 0)
        {
            var link = _ArrLink;
            if (link == null)
                return 0;

            token.ThrowIfCancellationRequested();
            var taskLink = _ArrLink as ITaskPlexBeaconPin<byte[]>;

            if (taskLink != null)
                link = await Task.Run(() => taskLink.ForNext, token);
            else
                link = await Task.Run(() => link.Next, token);

            Advance(link);
            return Read(buffer, offset, count);
        }

        return ReadCore(buffer, offset, r < count ? r : count);
    }
#endif
    public override bool CanSeek => false;
    public override bool CanWrite => false;
    public override long Length { get { throw new NotSupportedException(); } }
    public override long Position { get { throw new NotSupportedException(); } set { throw new NotSupportedException(); } }
    public override void Flush() { throw new NotSupportedException(); }
    public override long Seek(long offset, SeekOrigin origin) { throw new NotSupportedException(); }
    public override void SetLength(long value) { throw new NotSupportedException(); }
    public override void Write(byte[] buffer, int offset, int count) { throw new NotSupportedException(); }

    public PlexStream Clone() => new PlexStream(_ArrLink, _Position);
#if NETFX
    object ICloneable.Clone() => Clone();
#endif
    protected override void Dispose(bool disposing)
    {
        _ArrLink = null;
        _Data = null;
        _Position = 0;
    }
}
