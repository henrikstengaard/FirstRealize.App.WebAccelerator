using System.IO;

namespace FirstRealize.App.WebAccelerator.Filters
{
    public class CacheContentFilter : Stream
    {
        private readonly Stream _stream;
        private readonly MemoryStream _cacheStream;

        public CacheContentFilter(Stream stream)
        {
            _stream = stream;
            _cacheStream = new MemoryStream();
        }

        public override bool CanRead => _stream.CanRead;

        public override bool CanSeek => _stream.CanRead;

        public override bool CanWrite => _stream.CanRead;

        public override long Length => _stream.Length;

        public override long Position { get => _stream.Position; set => _stream.Position = value; }

        public override void Flush()
        {
            _stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
            _cacheStream.Write(buffer, offset, count);
        }

        public byte[] GetContent()
        {
            _cacheStream.Seek(0, SeekOrigin.Begin);
            var content = _cacheStream.ToArray();
            _cacheStream.Seek(0, SeekOrigin.End);
            return content;
        }
    }
}