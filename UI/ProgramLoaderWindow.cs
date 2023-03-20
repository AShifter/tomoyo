using System;
using System.IO;
using ImGuiNET;

namespace tomoyo.UI
{
    public static class ProgramLoaderWindow
    {
        public static void Show()
        {
	        ImGui.Begin("Program Loader"); 
	        ImGui.Text($"Total memory: {EmuState.GlobalMem.Memory.Length * 8} B ({Math.Round((double)EmuState.GlobalMem.Memory.Length * 8 / (double)1048576, 2)} MB)");
		        ImGui.Separator();
		        ImGui.InputTextWithHint("File Path","/", ref EmuState.loadFile, (uint)8192);
		        if (ImGui.InputInt($"@", ref EmuState.programLoadOffset, 256, 1024, ImGuiInputTextFlags.CharsHexadecimal))
		        {
			        if (EmuState.programLoadOffset < 0)
			        {
				        EmuState.programLoadOffset = 0;
			        }
		        }
		        
		        ImGui.RadioButton("plaintext (hsq)", ref EmuState.programFileType, 0);
		        ImGui.RadioButton("64-bit big endian (dawn)", ref EmuState.programFileType, 1);
		        ImGui.RadioButton("64-bit little endian (tomoyo)", ref EmuState.programFileType, 2);

		        string loadButtonName = "";
		        if (EmuState.programFileType == 0)
		        {
			        loadButtonName = "Load plaintext";
		        }
		        else if (EmuState.programFileType == 1)
		        {
			        loadButtonName = $"Load 64-bit big endian binary";
		        }
		        else if (EmuState.programFileType == 2)
		        {
			        loadButtonName = $"Load 64-bit little endian binary";
		        }

		        if (ImGui.Button(loadButtonName))
		        {
			        // Copy program into memory
			        try
			        {
				        EmuState.program = Helpers.SubleqLoader.LoadFromFile(EmuState.loadFile, (Helpers.ProgramFormat)EmuState.programFileType);
				        EmuState.program.CopyTo(EmuState.GlobalMem.Memory, EmuState.programLoadOffset);
				        //ProgramLoader.CopyToMemory(GlobalMem.WorkMemory, program, programLoadOffset);
			        }
			        catch(Exception e)
			        {
				        EmuState.errorPopup = true;
				        EmuState.errorException = e;
				        EmuState.errorTitle = "Could not load program";
			        }
		        }

		        if (EmuState.program != null)
		        {
			        if (ImGui.Button("Export loaded program as 64-bit LE binary"))
			        {
				        byte[] output = new byte[EmuState.program.Length * 8];
				        int x = 0;
				        for (int i = 0; i < output.Length; i += 8)
				        {
					        byte[] word = BitConverter.GetBytes(EmuState.program[x]);
					        x++;
					        output[i] = word[0];
					        output[i + 1] = word[1];
					        output[i + 2] = word[2];
					        output[i + 3] = word[3];
					        output[i + 4] = word[4];
					        output[i + 5] = word[5];
					        output[i + 6] = word[6];
					        output[i + 7] = word[7];
				        }

				        FileInfo info = new FileInfo(EmuState.loadFile);
				        File.WriteAllBytes($"{info.DirectoryName}/{info.Name}.bin", output);
			        }   
		        }

		        if (ImGui.Button("Clear"))
		        {
			        EmuState.GlobalMem.Clear();
			        Console.WriteLine("Cleared Memory");
		        }

		        ImGui.End();
        }
    }   
}