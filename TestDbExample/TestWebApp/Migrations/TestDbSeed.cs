using System;
using System.Threading.Tasks;
using TestWebApp.Models.Context;
using TestWebApp.Models.Entities;

namespace TestWebApp.Migrations
{
    public class TestDbSeed
    {
        public async Task<int> RunSeedAsync(TestDbContext context)
        {
            var random = new Random(new Guid().GetHashCode());
            var identifierEntity = new IdentifierEntity()
            {
                IsActive = random.Next(1, 100) > 50,
                Name = Guid.NewGuid().ToString("N"),
                Date = DateTime.UtcNow,
            };

            var sharedCount = random.Next(2, 5);
            for (int i = 0; i < sharedCount; i++)
            {
                var inheritedEntity = random.Next(0, 2);
                switch (inheritedEntity)
                {
                    case 0:
                        identifierEntity.SharedEntities.Add(new InheritedStringEntity()
                        {
                            EventType = "".PadRight(random.Next(1, 10), 'S'),
                            EventCount = random.Next(1000, 9999),
                            StringValue = "My String"
                        });
                        break;
                    case 1:
                        identifierEntity.SharedEntities.Add(new InheritedIntEntity()
                        {
                            EventType = "".PadRight(random.Next(1, 10), 'I'),
                            EventCount = random.Next(1000, 9999),
                            IntValue = random.Next(1000000, 9999999)
                        });
                        break;
                    case 2:
                        identifierEntity.SharedEntities.Add(new InheritedBoolEntity()
                        {
                            EventType = "".PadRight(random.Next(1, 10), 'B'),
                            EventCount = random.Next(1000, 9999),
                            BoolValue = random.Next(1, 100) > 50,
                        });
                        break;
                }
            }

            context.IdentifierEntities.Add(identifierEntity);
            await context.SaveChangesAsync();
            return identifierEntity.SharedEntities.Count;
        }
    }
}