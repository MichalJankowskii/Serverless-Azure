using System.Net;

public static HttpResponseMessage Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function - Subtraction.");

    int minuend   = int.Parse(req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "minuend", true) == 0)
        .Value);

    int subtrahend   = int.Parse(req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "subtrahend", true) == 0)
        .Value);

    return req.CreateResponse(HttpStatusCode.OK, minuend - subtrahend);
}