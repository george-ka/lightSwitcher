using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ArduinoLightswitcherGateway
{
    public class CircularBuffer<T> : ICollection<T>, IEnumerable<T>
    {
        public CircularBuffer(int bufferSize)
        {
            _bufferSize = bufferSize;
            _buffer = new T[_bufferSize];
            Clear();
        }

        public int Count => _total;

        public bool IsSynchronized => _buffer.IsSynchronized;

        public object SyncRoot => _buffer.SyncRoot;

        public bool IsReadOnly => _buffer.IsReadOnly;

        public void CopyTo(Array array, int index)
        {
            lock (_syncRoot)
            {
                _buffer.CopyTo(array, index);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new CircularBufferEnumerator<T>(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new CircularBufferEnumerator<T>(this);
        }

        public void Add(T item)
        {
            lock (_syncRoot)
            {
                _currentIndex = (_currentIndex + 1) % _buffer.Length;
                if (_total < _buffer.Length)
                {
                    _total++;
                }

                _buffer[_currentIndex] = item;
            }
        }

        public T Current
        {
            get 
            {
                lock (_syncRoot)
                {
                    if (!IsEmpty)
                    {
                        return _buffer[_currentIndex];
                    }
                }

                return default(T);
            }
        }

        public T Pop()
        {
            lock (_syncRoot)
            {
                if (IsEmpty)
                {
                    throw new Exception("Collection is empty");
                }

                var current = Current;

                _currentIndex--;
                if (_currentIndex < 0)
                {
                    _currentIndex = _buffer.Length - 1;
                }

                _total--;
                
                return current;
            }
        }

        public bool IsEmpty => _currentIndex < 0 && _total == 0;

        public void Clear()
        {
            lock (_syncRoot)
            {
                _currentIndex = -1;
                _total = 0;
            }
        }

        public bool Contains(T item) => _buffer.Any(i => i.Equals(item));

        public void CopyTo(T[] array, int arrayIndex)
        {
            _buffer.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        private readonly int _bufferSize;

        private int _currentIndex;

        private int _total;

        private readonly T[] _buffer;

        private object _syncRoot = new object();

        private class CircularBufferEnumerator<T1> : IEnumerator<T1>, IEnumerator
        {
            public CircularBufferEnumerator(CircularBuffer<T1> buffer)
            {
                if (buffer == null)
                {
                    throw new ArgumentException("Buffer must not be null", nameof(buffer));
                }

                _buffer = buffer;
                Reset();
            }

            public T1 Current  
            {
                get 
                {
                    if (_buffer._total == 0) 
                    {
                        return default(T1);
                    }
                    else
                    {
                        return _buffer._buffer[_currentIndex];  
                    }
                } 
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_iterations == 0)
                {
                    return false;
                }

                _currentIndex--;
                _iterations--;

                if (_currentIndex < 0)
                {
                    _currentIndex = _buffer._buffer.Length - 1;
                }

                return true;
            }

            public void Reset()
            {
                _iterations = _buffer._total;
                _currentIndex = _buffer._currentIndex + 1;
            }

            private readonly CircularBuffer<T1> _buffer;
            private int _currentIndex;
            private int _iterations;
        }
    }
}