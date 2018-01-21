using System.Net;

public static HttpResponseMessage Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function - Addition.");

    int summand1 = int.Parse(req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "summand1", true) == 0)
        .Value);

    int summand2 = int.Parse(req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "summand2", true) == 0)
        .Value);

    return req.CreateResponse(HttpStatusCode.OK, summand1 + summand2);
}