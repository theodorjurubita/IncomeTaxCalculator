using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Employees
{
    public class GetAll
    {
        public class Query : IRequest<List<Employee>>
        {
        }

        public class Handler : IRequestHandler<Query, List<Employee>>
        {
            private readonly HrContext _context;

            public Handler(HrContext context)
            {
                _context = context;
            }

            public async Task<List<Employee>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Employees.ToListAsync(cancellationToken);
            }
        }
    }
}
