using RentACarServer.Domain.Abstractions;
using RentACarServer.Domain.Shared;

namespace RentACarServer.Domain.Roles;

public sealed class Role : Entity
{
    public Role()
    {
        
    }

    public Role(Name name, bool isActive)
    {
        SetName(name);
        SetStatus(isActive);
    }
    public Name Name { get; set; } = default!;

    #region Behaviors

    public void SetName(Name name)
    {
        Name = name;
    }

    #endregion
}