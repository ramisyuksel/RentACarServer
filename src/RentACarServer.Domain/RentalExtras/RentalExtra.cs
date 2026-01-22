using RentACarServer.Domain.Abstractions;
using RentACarServer.Domain.Shared;

namespace RentACarServer.Domain.RentalExtras;

public sealed class RentalExtra : Entity
{
    public RentalExtra()
    {
    }

    public RentalExtra(Name name, Price price, Description description, bool isActive)
    {
        SetName(name);
        SetPrice(price);
        SetDescription(description);
        SetStatus(isActive);
    }

    public Name Name { get; private set; } = default!;
    public Price Price { get; private set; } = default!;
    public Description Description { get; private set; } = default!;

    #region Behaviors

    public void SetName(Name name) => Name = name;

    public void SetPrice(Price price) => Price = price;

    public void SetDescription(Description description) => Description = description;

    #endregion
}