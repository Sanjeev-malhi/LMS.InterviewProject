using LMS.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
                _context = context;
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
          return  await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
