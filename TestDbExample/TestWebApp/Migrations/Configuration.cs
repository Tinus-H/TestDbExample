using TestWebApp.Models.Context;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;

namespace TestWebApp.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<TestDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TestDbContext context)
        {
            var testDbSeed = new TestDbSeed();
            Task.Run(async () => await testDbSeed.RunSeedAsync(context)).GetAwaiter().GetResult();
        }
    }
}
