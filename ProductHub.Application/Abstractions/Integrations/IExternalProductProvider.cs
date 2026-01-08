using ProductHub.Application.DTOs.ExternalProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Application.Abstractions.Integrations;

public interface IExternalProductProvider
{
    Task<IReadOnlyCollection<ExternalProductDto>> GetProductsAsync(CancellationToken cancellationToken = default);
}
