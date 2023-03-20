using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Threading;
using tomoyo.Emulator;
using static SDL2.SDL;
using SDLImGuiGL;
using tomoyo.UI;

namespace tomoyo
{
    public class Program
    {
	    Thread sdlThread;
	    Thread cpuThread1;
	    private Stream inputStream;
	    
	    public static void Main(string[] args)
	    {
		    Console.WriteLine("ohayou gozaimasu!");
		    Program program = new Program();
		    program.SetupEmu();
	    }

	    public void SetupEmu()
	    {
		    EmuState.GlobalMem = new AddressableMemory(1024 * 1024 * 1024 / 8); // 1024 RAM (64-bit words have 8 bytes so we divide 1MB * 1024 by 8)
		    EmuState.FrameBufferSize = new Vector2(256, 256);
		    EmuState.FrameBufferOffset = 10240; // this is completely random, 0x2800
		    
		    EmuState.fb_w = (int)EmuState.FrameBufferSize.X;
		    EmuState.fb_h = (int)EmuState.FrameBufferSize.Y;
		    EmuState.fb_o = (int)EmuState.FrameBufferOffset;
		    EmuState.fb = new int[EmuState.fb_w * EmuState.fb_o];
		    
		    // Define threads
		    cpuThread1 = new Thread(StartCPU0);

		    EmuState.cpus = new CPU[1];
		    EmuState.cpus[0] = new CPU();

		    // Start SDL
		    StartSDL();
	    }

	    public static void StartCPU0()
	    {
		    EmuState.cpus[0] = new CPU();
		    EmuState.cpus[0].Name = "0";
		    EmuState.cpus[0].Output = new MemoryStream();
		    EmuState.cpus[0].Input = new StreamReader(Console.OpenStandardInput());
		    EmuState.cpus[0].Run(EmuState.GlobalMem, 0);
	    }

	    static ImGuiGLRenderer _renderer;
	    static bool _quit;
	    static IntPtr _window;
	    static IntPtr _glContext;

	    public void StartSDL()
	    {
		    // create a window, GL context and our ImGui renderer
	        (_window, _glContext) = ImGuiGL.CreateWindowAndGLContext("tomoyo", 800, 600);
	        _renderer = new ImGuiGLRenderer(_window, _glContext);

	        while (!_quit)
	        {
		        // send events to our window
		        while (SDL_PollEvent(out var e) != 0)
		        {
			        _renderer.ProcessEvent(e);
			        switch (e.type)
			        {
				        case SDL_EventType.SDL_QUIT:
				        {
					        _quit = true;
					        break;
				        }
				        case SDL_EventType.SDL_KEYDOWN:
				        {
					        switch (e.key.keysym.sym)
					        {
						        case SDL_Keycode.SDLK_ESCAPE:
							        _quit = true;
							        break;
					        }
					        break;
				        }
			        }
		        }
		        
		        _renderer.NewFrame();

		        // Copy framebuffer data to a 32-bit array - we read pixel data as RGBA8888 (32-bit) so reading the 64-bit memory directly would give us gaps in the image
		        unsafe
		        {
			        for (int i = 0; i < EmuState.fb.Length; i++)
			        {
				        EmuState.fb[i] = (int)EmuState.GlobalMem.Memory[i + EmuState.FrameBufferOffset];
			        }
			        
			        fixed(int* ptr = EmuState.fb)
			        {
				        EmuState.emuFB = (nint)ImGuiGL.LoadTexture((nint)ptr, (int)EmuState.FrameBufferSize.X, (int)EmuState.FrameBufferSize.Y, GL.PixelFormat.Rgba);
			        }
		        }

				ErrorWindow.Show();

				FrameBufferWindow.Show();

				CPUMonitorWindow.Show();

				ConsoleWindow.Show();

		        ProgramLoaderWindow.Show();
		        
		        _renderer.Render();
		        
		        SDL_GL_SwapWindow(_window);

		        GL.DeleteTexture((uint)EmuState.emuFB);
	        }
	        SDL_GL_DeleteContext(_glContext);
	        SDL_DestroyWindow(_window);
	        SDL_Quit();
	    }
    }
}