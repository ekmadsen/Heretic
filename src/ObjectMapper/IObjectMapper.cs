namespace ErikTheCoder.ObjectMapper;


public interface IObjectMapper
{
    IAsyncEnumerable<TDestination> Map<TSource, TDestination>(IAsyncEnumerable<TSource> sourceItems) where TDestination : class, new();
    IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> sourceItems) where TDestination : class, new();
    TDestination Map<TSource, TDestination>(TSource sourceItem) where TDestination : class, new();
    void Map<TSource, TDestination>(TSource sourceItem, TDestination destinationItem);
}