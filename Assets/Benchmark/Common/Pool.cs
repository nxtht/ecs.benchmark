using System;

namespace Benchmark
{
    public class Pool<T> where T : class
    {
        private int _count;
        private T[] _buffer;
        private Func<T> _factory;

        public Pool(Func<T> factory, int size = 8)
        {
            _factory = factory;
            size = Utils.GetNextPoT(size);
            _buffer = new T[size];
            for (var i = 0; i < size; i++)
            {
                _buffer[i] = _factory.Invoke();
            }

            _count = size;
        }

        public T Rent()
        {
            if (_count > 0)
            {
                _count--;
                var value = _buffer[_count];
                _buffer[_count] = null;
                return value;
            }

            return _factory.Invoke();
        }

        public void Return(T value)
        {
            if (_count == _buffer.Length)
            {
                Array.Resize(ref _buffer, _count * 2);
            }

            _buffer[_count++] = value;
        }
    }
}