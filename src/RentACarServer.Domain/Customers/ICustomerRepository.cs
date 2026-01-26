using RentACarServer.Domain.Abstractions;

namespace RentACarServer.Domain.Customers;

public interface ICustomerRepository : IAuditableRepository<Customer>
{
}