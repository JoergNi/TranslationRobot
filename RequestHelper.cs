using System.IO;
using System.Net;

public static class RequestHelper
{
    public static string DownloadString(string url)
    {


        HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();

        StreamReader responseStream = new StreamReader(objResponse.GetResponseStream());
        string responseRead = responseStream.ReadToEnd();

        responseStream.Close();
        responseStream.Dispose();

        return responseRead;
    }
}