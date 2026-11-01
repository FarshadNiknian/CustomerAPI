using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public sealed record CustomerCreatedDomainEvent(
        string FirstName,
        string LastName,
        string NationalCode) : DomainEvent;
}
