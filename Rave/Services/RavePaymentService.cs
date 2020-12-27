#region Licence

// // <copyright file="RavePaymentService.cs" company="Nervlite Designs">
// //  Copyright (c) 2020 All Rights Reserved
// // </copyright>
// // <author>Obichukwu Onyeowuzoni</author>
// // <date>// </date>
// // <summary></summary>

#endregion

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaveDotNet.Helpers;
using RaveDotNet.Models;
using RaveDotNet.Models.Events.Payment;
using RaveDotNet.Models.Payment;

namespace RaveDotNet.Services {
    public class RavePaymentService : RaveService
    {
        public string IntegrityHash { get; set; }

        protected readonly AdditionalRequestInfo AdditionalInfo;

        protected PaymentRequestModel Request { get; }

        public RavePaymentService(ConfigModel config, PaymentRequestModel request,AdditionalRequestInfo additionalInfo): base(config) {
            this.Request = request;
            this.AdditionalInfo = additionalInfo;
        }

        protected dynamic TransactionData =>
            new
            {
                PBFPubKey = this.Config.PublicKey,
                amount = this.Request.Amount,
                country = this.Request.Country,
                currency = this.Request.Currency,
                custom_description = this.Request.CustomDescription,
                custom_logo = this.Request.CustomLogo,
                custom_title = this.Request.CustomTitle,
                customer_email = this.Request.CustomerEmail,
                customer_firstname = this.Request.CustomerFirstname,
                customer_lastname = this.Request.CustomerLastname,
                customer_phone = this.Request.CustomerPhone,
                hosted_payment = 1,
                integrity_hash = this.IntegrityHash,
                meta = this.Config.Meta,
                pay_button_text = this.Request.PayButtonText,
                payment_method = this.Request.PaymentMethod,
                redirect_url = this.AdditionalInfo.RedirectUrl,
                txref = this.Request.TransactionReference
            };

        public string CreateCheckSum()
        {
            var sb = new StringBuilder();

            sb.Append(this.Config.PublicKey);
            sb.Append(Convert.ToString(Convert.ToInt32(this.Request.Amount)));
            sb.Append(this.Request.Country);
            sb.Append(this.Request.Currency);
            sb.Append(this.Request.CustomDescription);
            sb.Append(this.Request.CustomLogo);
            sb.Append(this.Request.CustomTitle);
            sb.Append(this.Request.CustomerEmail);
            sb.Append(this.Request.CustomerFirstname);
            sb.Append(this.Request.CustomerLastname);
            sb.Append(this.Request.CustomerPhone);
            sb.Append(1); //HostedPayment
            sb.Append(this.Request.PayButtonText);
            sb.Append(this.Request.PaymentMethod);
            sb.Append(this.AdditionalInfo.RedirectUrl);
            sb.Append(this.Request.TransactionReference);

            sb.Append(this.Config.SecretKey);

            var payload = sb.ToString();
            var bytes = Encoding.UTF8.GetBytes(payload);

            var transformedBytes = Hash.ComputeHash(bytes);

            return this.IntegrityHash = string.Join("", transformedBytes.Select(bt => bt.ToString("x2"))).ToLower();
        }

        public string RenderHtml()
        {
            this.CreateCheckSum();
            var transactionData = this.TransactionData;
            this.OnInit(new PaymentInitEventArgs());

            string body = Newtonsoft.Json.JsonConvert.SerializeObject(transactionData, Newtonsoft.Json.Formatting.Indented);

            return $@"<html>
                        <body>
                        <center>Processing...<br /><img src=""{AdditionalInfo.LoadingImage}"" /></center>
                        <script type=""text/javascript"" src='{this.Config.GetPaymentJsUrl()}'></script>
                        <script>
                            document.addEventListener(""DOMContentLoaded"", function(event) {{
                                var data = {body};
                                getpaidSetup(data);
                            }});
                        </script>
                        </body>
                    </html>";
        }

        public async Task<ResponseModel<PaymentResponseModel>> ReQueryTransaction(string transactionReference, PaymentRequestModel request = null)
        {
            var client = ApiClient.GetApiClient();
            this.ReQueryCount++;

            this.OnRequery(new PaymentRequeryEventArgs()
            {
                TransactionReference = transactionReference
            });
            var body = new
            {
                txref = transactionReference,
                SECKEY = this.Config.SecretKey,
                last_attempt = 1
            };

            try
            {
                var result = await client.Post<ResponseModel<PaymentResponseModel>>(this.Config.GetPaymentQueryUrl(), ApiClient.GetJsonContent<object>(body));

                if (result.IsSuccessful())
                {
                    this.OnSuccess(new PaymentSuccessEventArgs()
                    {
                        Request = request,
                        Response = result
                    });
                }
                else if (result.IsFailed())
                {
                    this.OnFailed(new PaymentFailedEventArgs()
                    {
                        Request = request,
                        Response = result
                    });
                }
                else
                {
                    if (this.ReQueryCount >= 4)
                    {
                        this.OnTimeout(new PaymentTimeoutEventArgs()
                        {
                            Request = request,
                            Response = result
                        });
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(3000); //wait for 3 seconds before retrying
                        return await this.ReQueryTransaction(transactionReference);
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                this.OnRequeryError(new PaymentRequeryErrorEventArgs()
                {
                    Error = ex
                });
                throw;
            }
        }

    }
}