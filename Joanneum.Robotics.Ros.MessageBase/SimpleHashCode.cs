namespace Joanneum.Robotics.Ros.MessageBase
{
    public struct SimpleHashCode
    {
        private int _hashCode;
        
        public void Add<T>(T value)
        {
            Add(value?.GetHashCode() ?? 0);
        }

        public void Add(int value)
        {
            _hashCode = (_hashCode * 389) ^ _hashCode;
        }

        public int ToHashCode()
        {
            return _hashCode;
        }
    }
}