using RaveDotNet.Events;
using RaveDotNet.Models.Events;
using System.Security.Cryptography;
using RaveDotNet.Models;

namespace RaveDotNet
{
    public class RaveService: RaveServiceEventHandler
    {
        protected ConfigModel Config { get; set; }
       
        protected SHA256 Hash;

        protected int ReQueryCount { get; set; }

        public RaveService(ConfigModel config) {
            this.Config = config;
            this.Hash = SHA256.Create();
        }
    }
}
