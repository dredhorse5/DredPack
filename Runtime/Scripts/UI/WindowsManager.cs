using System.Collections.Generic;
using DredPack.UI;

namespace DredPack.Runtime.Scripts.UI
{
    public class WindowsManager : GeneralSingleton<WindowsManager>
    {
        public static Dictionary<string, Window> RegisteredWindows { get; private set; } = new Dictionary<string, Window>();
        public static List<Window> AllWindows { get; private set; } = new List<Window>();
        

        public static bool TryGetWindow(string id, out Window window)
        {
            window = null;
            if (!string.IsNullOrEmpty(id) && RegisteredWindows.TryGetValue(id, out var registeredWindow))
                window = registeredWindow;
            

            return window;
        }

        public static void RegisterWindow(Window window)
        {
            if(!string.IsNullOrEmpty(window.RegisterID))
                RegisteredWindows.TryAdd(window.RegisterID, window);
            AllWindows.Add(window);
        }
        public static void UnRegisterWindow(Window window)
        {
            if(!string.IsNullOrEmpty(window.RegisterID) && RegisteredWindows.ContainsKey(window.RegisterID))
                RegisteredWindows.Remove(window.RegisterID);
            AllWindows.Remove(window);
        }
    }
}
