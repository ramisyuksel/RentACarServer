using Microsoft.EntityFrameworkCore;
using RentACarServer.Domain.Categories;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Categories;

public sealed record CategoryGetQuery(
    Guid Id) : IRequest<Result<CategoryDto>>;

internal sealed class CategoryGetQueryHandler(
    ICategoryRepository categoryRepository) : IRequestHandler<CategoryGetQuery, Result<CategoryDto>>
{
    public async Task<Result<CategoryDto>> Handle(CategoryGetQuery request, CancellationToken cancellationToken)
    {
        var res = await categoryRepository
            .GetAllWithAudit()
            .MapToGet()
            .Where(p => p.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
    
        if (res == null)
        {
            return Result<CategoryDto>.Failure("Category not found");
        }

        return res;
    }
}