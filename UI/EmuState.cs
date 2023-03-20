using System;
using System.IO;
using System.Numerics;
using System.Threading;
using tomoyo.Emulator;

namespace tomoyo.UI
{
    public static class EmuState
    {
        // Help
        public static IntPtr emuFB;
        public static string consoleBuffer = "";
        public static string input = "";
        public static string loadFile = "";
        public static int fb_w;
        public static int fb_h;
        public static int fb_o;
        public static int[] fb;
        public static string fbOffsetHex = "";
        public static int programFileType = 0; // 0 = plaintext, 1 = 64-bit big endian, 2 = 64-bit little endian
        public static int programLoadOffset = 0;
        public static long[] program;
        public static bool errorPopup = false;
        public static Exception errorException;
        public static string errorTitle = "";
        public static string inputBuffer = "";
        public static int cpu_rv = 0;
        public static AddressableMemory GlobalMem;
        public static Stream inputStream;
        public static long FrameBufferOffset;
        public static Vector2 FrameBufferSize;
        public static CPU[] cpus;
        public static Thread cpuThread0;
    }
}