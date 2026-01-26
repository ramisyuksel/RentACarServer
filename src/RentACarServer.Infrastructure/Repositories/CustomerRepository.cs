using RentACarServer.Domain.Customers;
using RentACarServer.Infrastructure.Context;
using RentACarServer.Infrastructure.Repositories.Abstractions;

namespace RentACarServer.Infrastructure.Repositories;

internal sealed class CustomerRepository(ApplicationDbContext context)
    : AuditableRepository<Customer, ApplicationDbContext>(context), ICustomerRepository;