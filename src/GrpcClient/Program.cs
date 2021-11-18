using System;
using Grpc.Core;
using Grpc.Net.Client;
using OzonEdu.MerchandiseService.Grpc;

using var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new MerchandiseServiceGrpc.MerchandiseServiceGrpcClient(channel);

try
{
    var result = await client.OrderMerchAsync(new OrderMerchGrpcRequest());
}
catch (RpcException e)
{
    Console.WriteLine(e);
}