using System.Numerics;
using ImGuiNET;
using SDLImGuiGL;

namespace tomoyo.UI
{
    public static class FrameBufferWindow
    {
        public static void Show()
        {
            ImGui.Begin("Framebuffer");
            EmuState.fbOffsetHex = string.Format("{0:x}", EmuState.FrameBufferOffset);
            ImGui.Text($"size = {(int)EmuState.FrameBufferSize.X}x{(int)EmuState.FrameBufferSize.Y}x32bpp @ 0x{EmuState.fbOffsetHex}");
            ImGui.Image(EmuState.emuFB, new Vector2((int)EmuState.FrameBufferSize.X, (int)EmuState.FrameBufferSize.Y));
            ImGui.Text("Set Framebuffer:");

            if (ImGui.InputInt("w", ref EmuState.fb_w, 8, 32))
            {
                if (EmuState.fb_w < 8)
                {
                    EmuState.fb_w = 8;
                }

                EmuState.FrameBufferSize = new Vector2(EmuState.fb_w, EmuState.fb_h);
                unsafe
                {
                    fixed (int* ptr = EmuState.fb)
                    {
                        EmuState.emuFB = (nint)ImGuiGL.LoadTexture((nint)ptr, (int)EmuState.FrameBufferSize.X,
                            (int)EmuState.FrameBufferSize.Y, GL.PixelFormat.Rgba);
                    }
                }
            }

            if (ImGui.InputInt("h", ref EmuState.fb_h, 8, 32))
            {
                if (EmuState.fb_h < 8)
                {
                    EmuState.fb_h = 8;
                }

                EmuState.FrameBufferSize = new Vector2(EmuState.fb_w, EmuState.fb_h);
                unsafe
                {
                    fixed (int* ptr = EmuState.fb)
                    {
                        EmuState.emuFB = (nint)ImGuiGL.LoadTexture((nint)ptr, (int)EmuState.FrameBufferSize.X,
                            (int)EmuState.FrameBufferSize.Y, GL.PixelFormat.Rgba);
                    }
                }
            }

            if (ImGui.InputInt($"@", ref EmuState.fb_o, 256, 1024, ImGuiInputTextFlags.CharsHexadecimal))
            {
                if (EmuState.fb_o < 0)
                {
                    EmuState.fb_o = 0;
                }

                EmuState.FrameBufferOffset = EmuState.fb_o;
            }

            ImGui.End();
        }
    }
}