using System;
using System.IO;

namespace tomoyo.Helpers
{
    public class SubleqLoader
    {
        /// <summary>
        /// Load machine code from a file on disk. The file can either be stored in plaintext format (as from hsq compiler) or in 64-bit binary (for DawnOS etc.) big endian/little endian
        /// </summary>
        /// <param name="path">The file to load.</param>
        /// <param name="format">The format of the data.</param>
        public static long[] LoadFromFile(string path, ProgramFormat format)
        {
            Console.WriteLine($"Loading program at {path}");
            FileStream stream = File.OpenRead(path);

            switch (format)
            {
                case ProgramFormat.PlainText:
                    string[] input = File.ReadAllLines(path);
                    string combined = "";
                    foreach (string num in input)
                    {
                        combined += num;
                    }

                    string[] split = combined.Split(" ");
            
                    long[] program = new long[split.Length];
            
                    for(int i = 0; i < split.Length; i++)
                    {
                        if (split[i] != "")
                        {
                            program[i] = long.Parse(split[i]);
                        }
                    }
            
                    return program;
                case ProgramFormat.BinBigEndian:
                    return ReadBigEndianBinary(stream);
                case ProgramFormat.BinLittleEndian:
                    return ReadLittleEndianBinary(stream);
                default:
                    throw new Exception();
            }
        }

        private static long[] ReadBigEndianBinary(Stream stream)
        {
            BinaryReaderBE reader = new BinaryReaderBE(stream);
            
            long[] program = new long[stream.Length / sizeof(long)];
            
            for (int i = 0; i < program.Length; i++)
            {
                program[i] = reader.ReadInt64();
            }
            
            return program;
        }

        private static long[] ReadLittleEndianBinary(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            
            long[] program = new long[stream.Length / sizeof(long)];
            
            for (int i = 0; i < program.Length; i++)
            {
                program[i] = reader.ReadInt64();
            }
            
            return program;
        }
    }
}