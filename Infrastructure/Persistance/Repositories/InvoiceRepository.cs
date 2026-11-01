using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistance.Repositories
{
    public sealed class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Invoice invoice, CancellationToken cancellationToken = default)
        {
            await _context.Invoices.AddAsync(invoice, cancellationToken);
        }

        public async Task<bool> ExistsByNumberAsync(string invoiceNumber, CancellationToken cancellationToken = default)
        {
            var numberVo = InvoiceNumber.Create(invoiceNumber);
            return await _context.Invoices
                .AnyAsync(x => x.Number == numberVo, cancellationToken);
        }
    }
}
