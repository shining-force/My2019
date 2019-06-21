using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace DatabaseConsole
{
    public class HttpHandlerException
    {
        public enum eFunctionException
        {
            OK,
            UsageError,
        }

        private bool mIsOK;
        private string mMessage;

        private Exception mHttpException;
        private eFunctionException mFunctionException;


        public HttpHandlerException(Exception httpException, eFunctionException funcException)
        {
            mHttpException = httpException;
            mFunctionException = funcException;

            if (mHttpException != null)//http优先
            {
                mMessage = mHttpException.Message;
                mIsOK = false;
            }
            else if (funcException != eFunctionException.OK)
            {
                switch (funcException)
                {
                    case eFunctionException.UsageError:
                        mMessage = "函数使用错误";
                        mIsOK = false;
                        break;
                    default:
                        mMessage = "未知错误";
                        mIsOK = false;
                        break;
                }
            }
            else
            {
                mMessage = "OK";
                mIsOK = true;
            }
        }

        public bool IsOK()
        {
            return mIsOK;
        }

        public string getMessage()
        {
            return mMessage;
        }

        public void getRawException(out Exception httpEx, out eFunctionException funcEx)
        {
            httpEx = mHttpException;
            funcEx = mFunctionException;
        }
    }

    public class HttpHandler
    {
        private static int sDefaultTimeOut = 5000;

        private string mUrl;

        public HttpHandler(string url)
        {
            mUrl = url;
        }

        public HttpHandlerException goSingle<R>(object requestParams, out R r, string method)
        {
            HttpWebRequest request;
            r = default(R);
            try
            {
                switch (method)
                {
                    case "GET":
                        request = createGetRequest(requestParams);
                        break;
                    case "POST":
                        request = createPostRequest(requestParams);
                        break;
                    default:
                        return new HttpHandlerException(null, HttpHandlerException.eFunctionException.UsageError);
                }

                r = handleResponse<R>(request);

                return new HttpHandlerException(null, HttpHandlerException.eFunctionException.OK);
            }
            catch (Exception e)
            {
                return new HttpHandlerException(e, HttpHandlerException.eFunctionException.OK);
            }            
        }

        public void goSingleAsync<R>(object t, string method, Action<R, HttpHandlerException> callback)
        {
            Thread asyncThread = new Thread(new ThreadStart(delegate {
                R r;
                HttpHandlerException e = goSingle(t, out r, method);
                callback(r, e);
            }));
            asyncThread.Start();
        }

        public void goCycle<R>(object t, string method, int cycleNum, Action<R, HttpHandlerException> cycleProcess)
        {
            Thread cycleThread = new Thread(new ThreadStart(delegate {
                while (cycleNum != 0)
                {
                    R r;
                    HttpHandlerException e = goSingle(t, out r, method);
                    cycleProcess(r, e);

                    --cycleNum;
                    if (cycleNum < 0)
                        cycleNum = -1;
                }
            }));
            cycleThread.Start();
        }

        public void goTimer<R>(object t, string method, int interval, Action<R, HttpHandlerException> timerProcess)
        {
            Timer timer = new Timer(delegate {
                R r;
                HttpHandlerException e = goSingle(t, out r, method);
                timerProcess(r, e);
            }, null, 0, interval);            
        }

        private R handleResponse<R>(HttpWebRequest request)
        {
            HttpWebResponse response;
            StreamReader reader;

            response = (HttpWebResponse)request.GetResponse();
            reader = new StreamReader(response.GetResponseStream());

            String buffer = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<R>(buffer);
        }

        private HttpWebRequest createGetRequest(object keyPairs)
        {
            HttpWebRequest request;

            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(mUrl).Append("?");
            foreach (KeyValuePair<string, string> pair in (Dictionary<string, string>)keyPairs)
            {
                urlBuilder.Append(pair.Key).Append("=").Append(pair.Value).Append("&");
            }

            string url = urlBuilder.ToString();
            request = (HttpWebRequest)WebRequest.Create(url.Remove(url.Length - 1));

            request.Timeout = sDefaultTimeOut;
            request.Method = "GET";
            request.ContentType = "application/json";

            return request;
        }

        private HttpWebRequest createPostRequest(object jsonClass)
        {
            HttpWebRequest request;

            request = (HttpWebRequest)WebRequest.Create(mUrl);
            request.Timeout = sDefaultTimeOut;
            request.Method = "POST";
            request.ContentType = "application/json";

            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            string json = JsonConvert.SerializeObject(jsonClass, jsSettings);

            Stream req = request.GetRequestStream();
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            req.Write(bytes, 0, bytes.Length);

            return request;
        }
    }
}
