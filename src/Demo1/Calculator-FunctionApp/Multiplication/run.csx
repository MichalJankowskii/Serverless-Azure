using System.Net;

public static HttpResponseMessage Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function - Multiplication.");

    int multiplicand = int.Parse(req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "multiplicand", true) == 0)
        .Value);

    int multiplier = int.Parse(req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "multiplier", true) == 0)
        .Value);

    return req.CreateResponse(HttpStatusCode.OK, (multiplicand * multiplier));
}