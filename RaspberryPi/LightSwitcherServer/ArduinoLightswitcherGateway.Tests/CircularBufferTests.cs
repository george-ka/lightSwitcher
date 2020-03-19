using System;
using System.Collections.Generic;
using Xunit;

namespace ArduinoLightswitcherGateway.Tests
{
    public class CircularBufferTests
    {
        [Fact]
        public void CircularBufferCanAddMoreValuesThanItsLength()
        {
            var buffer = new CircularBuffer<int>(BUFFER_SIZE);
            Add6Numbers(buffer);

            Assert.Equal(BUFFER_SIZE, buffer.Count);
        }

        [Fact]
        public void NewNumbersShouldOverrideOld()
        {
            var buffer = new CircularBuffer<int>(BUFFER_SIZE);
            Add6Numbers(buffer);
            var underlyingArray = new int[BUFFER_SIZE];
            buffer.CopyTo(underlyingArray, 0);
            Assert.Equal(new [] { 4, 5, 6 }, underlyingArray);
        }

        [Fact]
        public void PopWorksInReversedOrder()
        {
            var buffer = new CircularBuffer<int>(BUFFER_SIZE);
            Add6Numbers(buffer);
            var list = new List<int>();
            for (var i = 0; i< BUFFER_SIZE; i++)
                list.Add(buffer.Pop());

            var underlyingArray = new int[BUFFER_SIZE];
            buffer.CopyTo(underlyingArray, 0);
            Array.Reverse(underlyingArray);

            Assert.Equal(list.ToArray(), underlyingArray);
        }

        [Fact]
        public void EnumerationWorksInReversedOrder()
        {
            var buffer = new CircularBuffer<int>(BUFFER_SIZE);
            Add6Numbers(buffer);

            var list = new List<int>();
            foreach (var i in buffer)
                list.Add(buffer.Pop());

            var underlyingArray = new int[BUFFER_SIZE];
            buffer.CopyTo(underlyingArray, 0);
            Array.Reverse(underlyingArray);

            Assert.Equal(list.ToArray(), underlyingArray);
        }

        private void Add6Numbers(CircularBuffer<int> buffer)
        {
            buffer.Add(1);
            buffer.Add(2);
            buffer.Add(3);
            buffer.Add(4);
            buffer.Add(5);
            buffer.Add(6);
        }

        private const int BUFFER_SIZE = 3;
    }
}
