#region Licence

// // <copyright file="ResponseModel.cs" company="Nervlite Designs">
// //  Copyright (c) 2020 All Rights Reserved
// // </copyright>
// // <author>Obichukwu Onyeowuzoni</author>
// // <date>// </date>
// // <summary></summary>

#endregion

namespace RaveDotNet.Models {

    public class ResponseModel
    {
        public const string SUCCESS = "success";
        public const string FAILED = "failed";
    }

    public class ResponseModel<TData>
    {
        public string status { get; set; }
        public string message { get; set; }
        public TData data { get; set; }

        public bool IsSuccessful() {
            return this.status == ResponseModel.SUCCESS;
        }

        public bool IsFailed() {
            return this.status == ResponseModel.FAILED;
        }
    }
}