using ErikTheCoder.ObjectMapper;
using Entities = ErikTheCoder.Heretic.Contracts.Internal.Entities;
using Dtos = ErikTheCoder.Heretic.Contracts.Dtos;


namespace ErikTheCoder.Heretic.Core.Services;


public class ObjectMapper : ObjectMapperBase
{
    public override TDestination Map<TSource, TDestination>(TSource sourceItem)
    {
        // TODO: Investigate using Mapperly NuGet package to map TSource to TDestination.
        if ((sourceItem is Entities.User userEntity) && (typeof(TDestination) == typeof(Dtos.User))) return Map(userEntity) as TDestination;

        throw new ArgumentException($"Mapping from {typeof(TSource)} type to {typeof(TDestination)} type not supported.");
    }


    private static Dtos.User Map(Entities.User userEntity) => new()
    {
        Id = userEntity.Id,
        Username = userEntity.Username,
        Email = userEntity.Email,
        FirstName = userEntity.FirstName,
        LastName = userEntity.LastName
    };


    public override void Map<TSource, TDestination>(TSource sourceItem, TDestination destinationItem) =>
        throw new NotImplementedException($"Mapping from {sourceItem.GetType().FullName} type onto an existing {destinationItem.GetType().FullName} type not supported.");
}