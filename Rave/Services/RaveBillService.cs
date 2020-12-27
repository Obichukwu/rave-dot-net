#region Licence

// // <copyright file="RaveBillService.cs" company="Nervlite Designs">
// //  Copyright (c) 2020 All Rights Reserved
// // </copyright>
// // <author>Obichukwu Onyeowuzoni</author>
// // <date>// </date>
// // <summary></summary>

#endregion

using System;
using System.Threading.Tasks;
using RaveDotNet.Helpers;
using RaveDotNet.Models;
using RaveDotNet.Models.BillPayment;
using RaveDotNet.Models.Events;

namespace RaveDotNet.Services {
    public class RaveBillService : RaveService
    {
        public RaveBillService(ConfigModel config) : base(config)
        {
        }

        public async Task<ResponseModel<FlyBuyResponsePayload>> PurchaseAirtme(string phoneNumber, int amount, string reference) {
            return await FlyBuyTransaction(new FlyBuyRequestPayload
            {
                Country ="NG", 
                BillerName="AIRTIME", 
                IsAirtime =true,
                RecurringType = 0,
                CustomerId = phoneNumber, 
                Amount = amount,                 
                Reference= reference
            });
        }

        public async Task<ResponseModel<FlyBuyResponsePayload>> PurchaseDSTV(string smartcardNumber, int amount, string reference)
        {
            return await FlyBuyTransaction(new FlyBuyRequestPayload
            {
                Country = "NG",
                BillerName = "DSTV",
                IsAirtime = false,
                RecurringType = 0,
                CustomerId = smartcardNumber,
                Amount = amount,
                Reference = reference
            });
        }

        public async Task<ResponseModel<FlyBuyResponsePayload>> PurchaseDstvBoxOffice(string smartcardNumber, int amount, string reference)
        {
            return await FlyBuyTransaction(new FlyBuyRequestPayload
            {
                Country = "NG",
                BillerName = "DSTV BOX OFFICE",
                IsAirtime = false,
                RecurringType = 0,
                CustomerId = smartcardNumber,
                Amount = amount,
                Reference = reference
            });
        }

        public async Task<ResponseModel<FlyBuyResponsePayload>> FlyBuyTransaction(FlyBuyRequestPayload request)
        {
            var client = ApiClient.GetApiClient();

            this.OnRequery(new RequeryEventArgs()
            {
               
            });

            var body = BillPaymentRequestModel.GetFlyBuyRequestModel(Config.SecretKey, request);

            try
            {
                var result = await client.Post<ResponseModel<FlyBuyResponsePayload>>(this.Config.GetBillPaymentUrl(), ApiClient.GetJsonContent(body));

                if (result.IsSuccessful())
                {
                    this.OnSuccess(new SuccessEventArgs()
                    {
                    });
                }
                else if (result.IsFailed())
                {
                    this.OnFailed(new FailedEventArgs()
                    {
                    });
                }
                else
                {
                    if (this.ReQueryCount >= 4)
                    {
                        this.OnTimeout(new TimeoutEventArgs()
                        {
                        });
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(3000); //wait for 3 seconds before retrying
                        return await this.FlyBuyTransaction(request);
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                this.OnRequeryError(new RequeryErrorEventArgs()
                {
                    Error = ex
                });
                throw;
            }
        }


        public async Task<ResponseModel<FlyBuyResponsePayload[]>> FlyBuyBulkTransaction(FlyBuyBulkRequestPayload request)
        {
            var client = ApiClient.GetApiClient();

            this.OnRequery(new RequeryEventArgs()
            {

            });

            var body = BillPaymentRequestModel.GetFlyBuyBulkRequestModel(Config.SecretKey, request);

            try
            {
                var result = await client.Post<ResponseModel<FlyBuyResponsePayload[]>>(this.Config.GetUrl("flwv3-pug/getpaidx/api/xrequery"), ApiClient.GetJsonContent(body));

                if (result.IsSuccessful())
                {
                    this.OnSuccess(new SuccessEventArgs()
                    {
                    });
                }
                else if (result.IsFailed())
                {
                    this.OnFailed(new FailedEventArgs()
                    {
                    });
                }
                else
                {
                    if (this.ReQueryCount >= 4)
                    {
                        this.OnTimeout(new TimeoutEventArgs()
                        {
                        });
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(3000); //wait for 3 seconds before retrying
                        return await this.FlyBuyBulkTransaction(request);
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                this.OnRequeryError(new RequeryErrorEventArgs()
                {
                    Error = ex
                });
                throw;
            }
        }

    }
}