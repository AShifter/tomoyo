using System;
using System.IO;

namespace tomoyo.Emulator
{
    public class CPU
    {
        public long ProgramCounter { get => _pc; }
        public long RegisterA { get => _a; }
        public long RegisterB { get => _b; }
        public long RegisterC { get => _c; }
        public bool IsRunning { get => _running; }
        public MemoryStream Output { get => _output; set => _output = value; }
        public StreamReader Input { get => _input; set => _input = value; }
        public string Name { get; set; }

        private long _pc = 0;
        private long _a = 0;
        private long _b = 0;
        private long _c = 0;
        private bool _running = false;
        private MemoryStream _output;
        private StreamReader _input;

        public void Run(IAddressable _mem, long resetVector)
        {
            _pc = resetVector;
            _running = true;
            Console.WriteLine($"CPU {Name} Started");

            // CPU loop
            while (_pc >= 0)
            {
                // Load next three bytes of memory into registers
                _a = _mem.Read(_pc++);
                _b = _mem.Read(_pc++);
                _c = _mem.Read(_pc++);

                if (_a < 0)
                {
                    _mem.Write(_b, _input.Read()); // Read byte from input stream
                }
                
                else if (_b < 0)
                {
                    _output.WriteByte((byte)_mem.Read(_a)); // Send byte to output stream
                }
                
                else if (_mem.Write(_b, _mem.Read(_b) - _mem.Read(_a)) <= 0)
                {
                    _pc = _c; // Move the program counter to the value in register C
                }
            }
            _running = false;
            Console.WriteLine($"CPU {Name} Finished");
        }
    }
}