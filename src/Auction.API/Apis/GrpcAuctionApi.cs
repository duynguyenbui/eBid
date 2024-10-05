namespace eBid.Auction.API;

public class GrpcAuctionApi(AuctionContext context) : GrpcAuction.GrpcAuctionBase
{
    public override async Task<GrpcAuctionResponse> GetAuction(GetAuctionRequest request, ServerCallContext context1)
    {
        if (request.Id == 0)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Auction ID is required"));
        }

        var item = await context.AuctionItems
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == request.Id);

        if (item is null)
        {
            return new GrpcAuctionResponse();
        }

        var grpcAuctionModel = new GrpcAuctionModel
        {
            Id = item.Id,
            Seller = item.SellerId,
            AuctionEnd = item.EndingTime.ToString("o"),
            StartingPrice = item.StartingPrice.ToString("F2")
        };

        return new GrpcAuctionResponse { Auction = grpcAuctionModel };
    }
}