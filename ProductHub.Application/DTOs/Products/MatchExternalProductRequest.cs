using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Application.DTOs.Products;

public sealed class MatchExternalProductRequest
{
    public int ExternalProductId { get; init; }
}