using Microsoft.Extensions.Logging;
using ProductHub.Application.Abstractions.Integrations;
using ProductHub.Application.DTOs.ExternalProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Infrastructure.Integrations;

public sealed class FakeStoreApiClient : IExternalProductProvider
{
    private const string ProductsEndpoint = "products";

    private readonly HttpClient _httpClient;
    private readonly ILogger<FakeStoreApiClient> _logger;

    public FakeStoreApiClient(
        HttpClient httpClient,
        ILogger<FakeStoreApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<ExternalProductDto>> GetProductsAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(ProductsEndpoint, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("FakeStore API returned non-success status code: {StatusCode}", response.StatusCode);

                return Array.Empty<ExternalProductDto>();
            }

            var products = await response.Content.ReadFromJsonAsync<List<ExternalProductDto>>(cancellationToken)
                ?? new List<ExternalProductDto>();

            return products;
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("FakeStore API request was cancelled.");

            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while calling FakeStore API.");

            return Array.Empty<ExternalProductDto>();
        }
    }
}
