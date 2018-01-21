using System.Net;

public static HttpResponseMessage Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function - Division.");

    int dividend  = int.Parse(req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "dividend", true) == 0)
        .Value);

    int divisor  = int.Parse(req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "divisor", true) == 0)
        .Value);

    if (divisor == 0)
    {
        return req.CreateResponse(HttpStatusCode.BadRequest, "Divisor cannot be 0");    
    }

    return req.CreateResponse(HttpStatusCode.OK, dividend / divisor);
}