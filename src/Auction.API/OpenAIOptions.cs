namespace eBid.Auction.API;

public class OpenAIOptions
{
    public string Model { get; set; }
    public string ApiKey { get; set; }
    public string Endpoint { get; set; }

    public override string ToString()
    {
        return $"{nameof(Model)}: {Model}, {nameof(ApiKey)}: {ApiKey}, {nameof(Endpoint)}: {Endpoint}";
    }
}