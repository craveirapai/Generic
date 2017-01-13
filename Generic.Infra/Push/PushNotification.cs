using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using PushSharp.Google;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Generic.Infra.Push
{
    public class PushDevice
    {
        public String Token { get; set; }
        public int Type { get; set; }
    }

    public class PushMessage
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public List<PushDevice> Device { get; set; } = new List<PushDevice>();

        public String ExtraData { get; set; }
    }

    public enum PushMessageType
    {
        Generic = 0
    }

    public class PushNotification
    {

        ApnsConfiguration AppleConfig { get; set; }
        GcmConfiguration GoogleConfiguration { get; set; }


        public PushNotification()
        {




            var googlePassword = ConfigurationManager.AppSettings["GcmKey"].ToString();
            this.GoogleConfiguration = new GcmConfiguration(googlePassword);

        }


        public void SendMessage(PushMessage message)
        {

            foreach (var token in message.Device)
            {
                if (token.Type == 1)
                {
                    SendNotificationForApple(message, token);
                }

                if (token.Type == 2)
                {
                    SendNotificationForGoogle(message, token);
                }

                if (token.Type == 3)
                {
                    SendNotificationForApple(message, token, true);
                }
            }

        }


        

        private void SendNotificationForApple(PushMessage message, PushDevice token, bool InProduction = false)
        {


            var pathCert = (InProduction ? "Generic.Infra.Push.APNS.push_prod_key.p12" : "Generic.Infra.Push.APNS.push_dev_key.p12");
            var enviroment = (InProduction ? ApnsConfiguration.ApnsServerEnvironment.Production : ApnsConfiguration.ApnsServerEnvironment.Sandbox);

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(pathCert);
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            var appleCert = ms.ToArray();

            //Lê o certificado Apple
            var applePassword = ConfigurationManager.AppSettings["APNS_PASSWORD"].ToString();

            this.AppleConfig = new ApnsConfiguration(enviroment, appleCert, applePassword, false);
            var apnsBroker = new ApnsServiceBroker(this.AppleConfig);

            apnsBroker.Start();

            var aps = new
            {
                aps = new
                {
                    alert = new
                    {
                        title = message.Title,
                        body = message.Body
                    }
                },
                data = new
                {
                    extra = message.ExtraData,
                }
            };

            apnsBroker.QueueNotification(new ApnsNotification()
            {
                DeviceToken = token.Token,
                Payload = JObject.Parse(JsonConvert.SerializeObject(aps))
                
            });

            apnsBroker.Stop();
        }



        private void SendNotificationForGoogle(PushMessage message, PushDevice token)
        {
           
            var gcm = new
            {
                gcm = new
                {
                    alert = new
                    {
                        title = message.Title,
                        body = message.Body
                    }
                },
                data = new
                {
                    extra = message.ExtraData,
                }
            };

            SendNotificationJson(ConfigurationManager.AppSettings["GcmKey"].ToString(), ConfigurationManager.AppSettings["GcmSenderId"].ToString(), token.Token.ToString(), message).Response.ToString();
        }

        public AndroidFCMPushNotificationStatus SendNotificationJson(string serverApiKey, string senderId, string deviceId, PushMessage message)
        {
            AndroidFCMPushNotificationStatus result = new AndroidFCMPushNotificationStatus();

            try
            {
                result.Successful = false;
                result.Error = null;

                var value = message;
                WebRequest tRequest = WebRequest.Create(ConfigurationManager.AppSettings["GcmUrl"].ToString());
                tRequest.Method = "post";
               
                tRequest.ContentType = "application/json";

                var gcm = new
                {
                    to = deviceId,
                    notification = new
                    {
                        body = message.Body,
                        title = message.Title
                    },
                    data = new
                    {
                        extra = message.ExtraData
                    },
                };



                tRequest.Headers.Add(string.Format("Authorization: key={0}", serverApiKey));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));

               

                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(gcm);


                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                result.Response = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Successful = false;
                result.Response = null;
                result.Error = ex;
            }

            return result;
        }



        public AndroidFCMPushNotificationStatus SendNotification(string serverApiKey, string senderId, string deviceId, string message)
        {
            AndroidFCMPushNotificationStatus result = new AndroidFCMPushNotificationStatus();

            try
            {
                result.Successful = false;
                result.Error = null;

                var value = message;
                WebRequest tRequest = WebRequest.Create(ConfigurationManager.AppSettings["GcmUrl"].ToString());
                tRequest.Method = "post";
                tRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", serverApiKey));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));

                string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + value + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + deviceId + "";

                Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                result.Response = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Successful = false;
                result.Response = null;
                result.Error = ex;
            }

            return result;
        }


        public class AndroidFCMPushNotificationStatus
        {
            public bool Successful
            {
                get;
                set;
            }

            public string Response
            {
                get;
                set;
            }
            public Exception Error
            {
                get;
                set;
            }
        }






    }
}
