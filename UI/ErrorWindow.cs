using ImGuiNET;

namespace tomoyo.UI
{
    public static class ErrorWindow
    {
        public static void Show()
        {
            if (EmuState.errorPopup)
            {
                ImGui.Begin(EmuState.errorTitle);
                ImGui.TextWrapped(EmuState.errorException.Message);
                if (ImGui.Button("OK"))
                {
                    EmuState.errorPopup = false;
                    EmuState.errorException = null;
                    EmuState.errorTitle = "";
                }
                ImGui.End();
            }
        }
    }
}