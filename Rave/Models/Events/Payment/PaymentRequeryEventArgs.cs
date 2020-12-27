#region Licence

// // <copyright file="PaymentRequeryEventArgs.cs" company="Nervlite Designs">
// //  Copyright (c) 2020 All Rights Reserved
// // </copyright>
// // <author>Obichukwu Onyeowuzoni</author>
// // <date>// </date>
// // <summary></summary>

#endregion

namespace RaveDotNet.Models.Events.Payment {
    public class PaymentRequeryEventArgs : RequeryEventArgs
    {
        public string TransactionReference { get; set; }
    }
}