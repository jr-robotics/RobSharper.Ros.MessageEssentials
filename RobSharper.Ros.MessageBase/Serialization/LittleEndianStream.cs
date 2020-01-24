using System;
using System.IO;

namespace RobSharper.Ros.MessageBase.Serialization
{
    internal class LittleEndianStream : Stream
    {
        public enum WriteModes
        {
            Convert,
            DoNotConvert
        }

        public class ModeContext : IDisposable
        {
            private readonly LittleEndianStream _stream;
            private readonly WriteModes _resetNode;

            public ModeContext(LittleEndianStream stream, WriteModes resetNode)
            {
                _stream = stream;
                _resetNode = resetNode;
            }
            
            public void Dispose()
            {
                _stream.Mode = _resetNode;
            }
        }
        
        private readonly Stream _innerStream;

        public override bool CanRead => _innerStream.CanRead;
        public override bool CanSeek => _innerStream.CanSeek;
        public override bool CanWrite => _innerStream.CanWrite;
        public override long Length => _innerStream.Length;
        public override long Position
        {
            get => _innerStream.Position;
            set => _innerStream.Position = value;
        }

        private bool _mustReverse;
        private WriteModes _mode;

        public WriteModes Mode
        {
            get => _mode;
            set
            {
                _mustReverse =  value == WriteModes.Convert && !BitConverter.IsLittleEndian;
                _mode = value;
            }
        }

        public LittleEndianStream(Stream innerStream)
        {
            _innerStream = innerStream;
            _mode = WriteModes.Convert;
            _mustReverse = !BitConverter.IsLittleEndian;
        }
        public override void Flush()
        {
            _innerStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var bytesRead = _innerStream.Read(buffer, offset, count);

            if (_mustReverse)
            {
                Array.Reverse(buffer, offset, bytesRead);
            }

            return bytesRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _innerStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _innerStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (_mustReverse)
            {
                Array.Reverse(buffer, offset, count);
            }

            _innerStream.Write(buffer, offset, count);
        }

        public IDisposable UseMode(WriteModes mode)
        {
            if (mode == _mode)
                return NullDisposable.Instance;
            
            Mode = mode;
            return new ModeContext(this, mode);
        }

        public IDisposable DoNotConvert()
        {
            return UseMode(WriteModes.DoNotConvert);
        }

        public IDisposable Convert()
        {
            return UseMode(WriteModes.Convert);
        }
    }
}