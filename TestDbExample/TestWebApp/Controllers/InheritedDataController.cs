using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ExpressMapper.Extensions;
using TestWebApp.Common.Dtos;
using TestWebApp.Migrations;
using TestWebApp.Models.Context;
using TestWebApp.Models.Entities;

namespace TestWebApp.Controllers
{
    [RoutePrefix("api/InheritedData")]
    public class InheritedDataController : ApiController
    {
        private readonly TestDbContext _context;

        public InheritedDataController()
        {
            _context = new TestDbContext();
        }

        [Route("GetEntities"), ResponseType(typeof(IEnumerable<InheritedSharedEntity>))]
        public IHttpActionResult GetEntities()
        {
            return Ok(_context.InheritedSharedEntities.AsNoTracking());
        }

        [Route("GetProjectDtos"), ResponseType(typeof(IEnumerable<InheritedSharedEntityDto>))]
        public IHttpActionResult GetProjectDtos()
        {
            return Ok(_context.InheritedSharedEntities.AsNoTracking()
                .Project<InheritedSharedEntity, InheritedSharedEntityDto>());
        }

        [Route("GetMapDtos"), ResponseType(typeof(IEnumerable<InheritedSharedEntityDto>))]
        public IHttpActionResult GetMapDtos()
        {
            return Ok(_context.InheritedSharedEntities.AsNoTracking()
                .Map<IEnumerable<InheritedSharedEntity>, IEnumerable<InheritedSharedEntityDto>>());
        }

        [Route("Seed"), ResponseType(typeof(string))]
        public async Task<IHttpActionResult> SeedAsync()
        {
            var testDbSeed = new TestDbSeed();
            var sharedEntitiesCount = await testDbSeed.RunSeedAsync(_context);
            return Ok($"{sharedEntitiesCount} InheritedIntEntities Added");
        }
    }
}
