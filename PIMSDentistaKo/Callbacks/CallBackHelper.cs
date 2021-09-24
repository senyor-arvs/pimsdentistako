using System;
using System.Collections.Generic;
using System.Text;
using pimsdentistako.Callbacks;
using System.Windows;

namespace pimsdentistako.Callbacks
{
    public class CallBackHelper
    {

        public static class WindowCloseHelper
        {
            public static void OnLoadedConfig(Window active_class, IWindowCloseListener windowCloseListener)
            {
                active_class.Topmost = true;
                windowCloseListener.OnWindowClose(false);
            }

            public static void OnCloseConfig(Window active_class, IWindowCloseListener windowCloseListener)
            {
                active_class.Topmost = false;
                windowCloseListener.OnWindowClose(true);
            }

        }
    }
}
