using Domain.Entities;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer customer, CancellationToken cancellationToken = default);
        Task<bool> ExistsByNationalCodeAsync(NationalCode nationalCode, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(long id, CancellationToken cancellationToken = default);

    }
}
