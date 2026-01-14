using RentACarServer.Domain.Abstractions;
using RentACarServer.Domain.Branches.ValueObjects;
using RentACarServer.Domain.Shared;

namespace RentACarServer.Domain.Branches;

public class Branch : Entity
{

    private Branch() { }
    public Branch(
        Name name,
        Address address,
        bool isActive)
    {
        SetName(name);
        SetAddress(address);
        SetStatus(isActive);
    }
    public Name Name { get; private set; } = default!;
    public Address Address { get; private set; } = default!;

    #region Behaviors

    public void SetName(Name name)
    {
        Name = name;
    }

    public void SetAddress(Address address)
    {
        Address = address;
    }

    #endregion
}