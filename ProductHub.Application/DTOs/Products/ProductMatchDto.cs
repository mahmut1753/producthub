using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Application.DTOs.Products;

public class ProductMatchDto
{
    public int LocalProductId { get; set; }
    public int ExternalProductId { get; set; }
    public string LocalName { get; set; } = string.Empty;
    public string ExternalName { get; set; } = string.Empty;
    public decimal LocalPrice { get; set; }
    public decimal ExternalPrice { get; set; }

    public decimal PriceDifference => ExternalPrice - LocalPrice;
}
