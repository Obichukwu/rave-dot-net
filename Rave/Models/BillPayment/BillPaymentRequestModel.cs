#region Licence

// // <copyright file="BillPaymentRequestModel.cs" company="Nervlite Designs">
// //  Copyright (c) 2020 All Rights Reserved
// // </copyright>
// // <author>Obichukwu Onyeowuzoni</author>
// // <date>// </date>
// // <summary></summary>

#endregion

using System;

namespace RaveDotNet.Models.BillPayment {

    public class BillPaymentRequestModel
    {
        public string secret_key { get; set; }
        public string service { get; set; } //fly_buy
        public string service_method { get; set; } //post
        public string service_version { get; set; } //v1
        public string service_channel { get; set; } //rave
        public BillPaymentRequestPayload service_payload { get; set; }

        public static BillPaymentRequestModel GetFlyBuyRequestModel(string secretKey, FlyBuyRequestPayload payload) {
            return new BillPaymentRequestModel {
                secret_key = secretKey,
                service = "fly_buy", 
                service_method = "post", 
                service_version = "v1", 
                service_channel = "rave",
                service_payload = payload
            };
        }

        public static BillPaymentRequestModel GetFlyBuyBulkRequestModel(string secretKey, FlyBuyBulkRequestPayload payload)
        {
            return new BillPaymentRequestModel
            {
                secret_key = secretKey,
                service = "fly_buy_bulk",
                service_method = "post",
                service_version = "v1",
                service_channel = "rave",
                service_payload = payload
            };
        }
    }

    public class BillPaymentRequestPayload
    {
    }
}