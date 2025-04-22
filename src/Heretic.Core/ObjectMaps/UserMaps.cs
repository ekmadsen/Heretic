using Riok.Mapperly.Abstractions;
using Entities = ErikTheCoder.Heretic.Contracts.Internal.Entities;
using Dtos = ErikTheCoder.Heretic.Contracts.Dtos;


// ReSharper disable once CheckNamespace
namespace ErikTheCoder.Heretic.Core.Services;


[Mapper]
public partial class ObjectMapper
{
    public async IAsyncEnumerable<Dtos.User> ToDtos(IAsyncEnumerable<Entities.User> users)
    {
        await foreach (var user in users)
            yield return ToDto(user);
    }


    public partial Dtos.User ToDto(Entities.User source);
}