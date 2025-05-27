namespace VKR_server.Options;

public record AiOptions
{
    public required string ModelName { get; init; }
    public required string TogetherAiApiKey { get; init; }
}
