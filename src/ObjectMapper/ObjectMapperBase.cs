namespace ErikTheCoder.ObjectMapper;


public abstract class ObjectMapperBase : IObjectMapper
{
    public async IAsyncEnumerable<TDestination> Map<TSource, TDestination>(IAsyncEnumerable<TSource> sourceItems) where TDestination : class, new()
    {
        await foreach (var sourceItem in sourceItems)
            yield return Map<TSource, TDestination>(sourceItem);
    }


    public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> sourceItems) where TDestination : class, new()
    {
        foreach (var sourceItem in sourceItems)
            yield return Map<TSource, TDestination>(sourceItem);
    }


    public abstract TDestination Map<TSource, TDestination>(TSource sourceItem) where TDestination : class, new();
    public abstract void Map<TSource, TDestination>(TSource sourceItem, TDestination destinationItem);
}
