using System.Linq;

namespace tomoyo.Emulator
{
    public class AddressableMemory : IAddressable
    {
        public long[] Memory { get => _mem; }
        private long[] _mem;

        public AddressableMemory(long memorySize)
        {
            // Allocate emulated memory
            _mem = new long[memorySize];
        }

        /// <summary>
        /// Clear the internal memory array.
        /// </summary>
        public void Clear()
        {
            for (long i = 0; i < _mem.Length; i++)
            {
                _mem[i] = 0;
            }
        }
        
        public bool Contains(long addr)
        {
            return _mem.Contains(addr);
        }

        public long Read(long addr)
        {
            return _mem[addr];
        }

        public long Write(long addr, long val)
        {
            _mem[addr] = val;
            return val;
        }
    }
}