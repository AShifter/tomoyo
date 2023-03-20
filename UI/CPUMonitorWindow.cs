using System.IO;
using System.Threading;
using ImGuiNET;
using tomoyo.Emulator;

namespace tomoyo.UI
{
    public static class CPUMonitorWindow
    {
        public static void Show()
        {
            if (EmuState.cpus[0].Output != null)
            {
                using (var reader = new StreamReader(EmuState.cpus[0].Output)) 
                {
                    EmuState.cpus[0].Output.Position = 0; 
                    EmuState.consoleBuffer += reader.ReadToEnd();
                    EmuState.cpus[0].Output.Flush(); 
                    EmuState.cpus[0].Output = new MemoryStream();
                }
            }
            
            ImGui.Begin($"CPU Monitor");
            ImGui.Text($"CPU {EmuState.cpus[0].Name}:\nA: {EmuState.cpus[0].RegisterA}\nB: {EmuState.cpus[0].RegisterB}\nC: {EmuState.cpus[0].RegisterC}\nPC: {EmuState.cpus[0].ProgramCounter}\nRunning: {EmuState.cpus[0].IsRunning}");
		        
            if (ImGui.InputInt($"@", ref EmuState.cpu_rv, 256, 1024, ImGuiInputTextFlags.CharsHexadecimal))
            {
                if (EmuState.cpu_rv < 0)
                {
                    EmuState.cpu_rv = 0;
                }
            }
		        
            if (ImGui.Button("Start") && !EmuState.cpus[0].IsRunning)
            {
                EmuState.cpuThread0 = new Thread(Program.StartCPU0);
                EmuState.cpuThread0.Start();
            }
		        
            if (ImGui.Button("Destroy") && EmuState.cpus[0].IsRunning)
            {
                EmuState.cpus[0] = new CPU();
            }
		        
            ImGui.End();
        }
    }
}