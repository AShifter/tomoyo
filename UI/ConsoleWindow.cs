using System.IO;
using ImGuiNET;
using static tomoyo.UI.EmuState;

namespace tomoyo.UI
{
    public static class ConsoleWindow
    {
        public static void Show()
        {
            
            ImGui.Begin("Console");
            ImGui.TextWrapped(consoleBuffer);
            
            /*
            ImGui.Separator();
            if (ImGui.InputTextWithHint("Input", "", ref inputBuffer, 8192, ImGuiInputTextFlags.EnterReturnsTrue))
            {
                var streamWriter = new StreamWriter(inputStream);
                streamWriter.WriteLine(inputBuffer);
			        
                
                foreach (byte character in inputBuffer)
                {
                    inputStream.WriteByte(character);
                }

                inputBuffer = "";
            }
            */
		        
            if(ImGui.GetScrollY() >= ImGui.GetScrollMaxY()) { ImGui.SetScrollHereY(1.0f); }
            ImGui.End();
        }
    }
}