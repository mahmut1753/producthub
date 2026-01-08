using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Application.DTOs.ExternalProduct;

public class ExternalProductRatingDto
{
    public decimal Rate { get; set; }
    public int Count { get; set; }
}