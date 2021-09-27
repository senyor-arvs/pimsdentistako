using System;
using System.Collections.Generic;
using System.Text;

namespace pimsdentistako.Callbacks
{
    public interface IWindowCloseListener
    {
        void OnWindowClose(bool isClosed);
    }
}
