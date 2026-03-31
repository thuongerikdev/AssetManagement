using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TH.Asset.Infrastructure.Database;

namespace TH.Asset.ApplicationService.Common
{
    public class AssetServiceBase
    {
        private readonly ILogger _logger;

        private readonly AssetDbContext _dbContext;

        public AssetServiceBase(ILogger logger, AssetDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
    }

}
