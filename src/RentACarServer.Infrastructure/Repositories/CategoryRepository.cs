using RentACarServer.Domain.Categories;
using RentACarServer.Infrastructure.Context;
using RentACarServer.Infrastructure.Repositories.Abstractions;

namespace RentACarServer.Infrastructure.Repositories;

public class CategoryRepository(ApplicationDbContext context)
    : AuditableRepository<Category, ApplicationDbContext>(context), ICategoryRepository;