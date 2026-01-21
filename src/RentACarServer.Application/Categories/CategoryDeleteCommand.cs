using GenericRepository;
using RentACarServer.Application.Behaviors;
using RentACarServer.Domain.Categories;
using TS.MediatR;
using TS.Result;

namespace RentACarServer.Application.Categories;

[Permission("category:delete")]
public sealed record CategoryDeleteCommand(Guid Id) : IRequest<Result<string>>;

internal sealed class CategoryDeleteCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork
) : IRequestHandler<CategoryDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.FirstOrDefaultAsync(
            c => c.Id == request.Id,
            cancellationToken);

        if (category is null)
        {
            return Result<string>.Failure("Category not found");
        }

        category.Delete();
        categoryRepository.Update(category);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Category deleted successfully";
    }
}