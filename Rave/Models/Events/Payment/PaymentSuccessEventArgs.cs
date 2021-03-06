#region Licence

// // <copyright file="PaymentSuccessEventArgs.cs" company="Nervlite Designs">
// //  Copyright (c) 2020 All Rights Reserved
// // </copyright>
// // <author>Obichukwu Onyeowuzoni</author>
// // <date>// </date>
// // <summary></summary>

#endregion

using RaveDotNet.Models.Payment;

namespace RaveDotNet.Models.Events.Payment {
    public class PaymentSuccessEventArgs : SuccessEventArgs
    {
        public ResponseModel<PaymentResponseModel> Response { get; set; }
        public PaymentRequestModel Request { get; set; }
    }
}