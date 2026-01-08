using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Infrastructure.Persistence;

public class DapperContext : IDapperContext
{
    private readonly IConfiguration _configuration;

    public DapperContext(IConfiguration configuration)
         =>  _configuration = configuration;

    public IDbConnection CreateConnection()
        => new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));
}