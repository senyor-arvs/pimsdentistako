using System;
using System.Collections.Generic;
using System.Text;

namespace pimsdentistako.Callbacks
{
    public interface IOnDeleteSuccessListener
    {
        void OnDeleteSuccess(bool deleted, string owner);
    }
}
