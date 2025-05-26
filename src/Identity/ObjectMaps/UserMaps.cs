using ErikTheCoder.Utilities.Extensions;
using Riok.Mapperly.Abstractions;
using Entities = ErikTheCoder.Contracts.Internal.Entities;
using Dtos = ErikTheCoder.Contracts.Dtos;


// ReSharper disable once CheckNamespace
namespace ErikTheCoder.Identity.Services;


[Mapper]
public partial class ObjectMapper
{
    public IAsyncEnumerable<Dtos.User> ToDtos(IAsyncEnumerable<Entities.User> users) => users.EnumerateAndMap(ToDto);


    public partial Dtos.User ToDto(Entities.User source);
}