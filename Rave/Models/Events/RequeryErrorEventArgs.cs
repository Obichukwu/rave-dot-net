using System;

namespace RaveDotNet.Models.Events
{
    public class RequeryErrorEventArgs : EventArgs
    {
        public Exception Error { get; set; }   
    }
}
