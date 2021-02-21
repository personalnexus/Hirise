using Microsoft.VisualStudio.TestTools.UnitTesting;
using HiriseLib.Redis;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace HiriseTest
{
    [TestClass]
    public class Demo
    {
        [TestMethod]
        public async Task Hash()
        {
            IDatabase db = new Connection().GetInternalDatabase(); 
            var userKey = new RedisKey("Demo:Users:Stefan");

            // Check existance of a particular field
            if (!await db.HashExistsAsync(userKey, "FirstLoginTimestamp"))
            {
                await db.HashSetAsync(userKey, new HashEntry[]
                {
                    new HashEntry("FirstLoginTimestamp", DateTime.Now.ToString("O")),
                });
            }

            // Set several fields at once
            await db.HashSetAsync(userKey, new HashEntry[]
            {
                new HashEntry("LastLoginTimestamp", DateTime.Now.ToString("O")),
                new HashEntry("LastLoginMachine", Environment.MachineName)
            });
            
            // Atomic increment
            await db.HashIncrementAsync(userKey, "CurrentLogins");
            HashEntry[] keys = await db.HashGetAllAsync(userKey);
            
            // Get all fields of a hash
            foreach (var key in keys)
            {
                Console.WriteLine($"{key.Name}\t{key.Value}");
            }
            /* Output:
            LastLoginTimestamp  2021-02-21T12:04:18.6081131+01:00
            LastLoginMachine    ELAGABALUS
            CurrentLogins       1
            FirstLoginTimestamp 2021-02-21T11:59:35.1709573+01:00
            */

            // Delete a single field
            await db.HashDeleteAsync(userKey, "CurrentLogins");
        }
    }
}