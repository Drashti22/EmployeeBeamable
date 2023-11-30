using Beamable.Common;
using Beamable.Server;
using MongoDB.Driver;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Beamable.testing
{
    [Microservice("testing")]
    public class testing : Microservice
    {
        [ClientCallable]
        public async Promise<bool> SaveMessage(string message, int x, int y)
        {
            bool isSuccess = false;

            try
            {
                var db = await Storage.GetDatabase<testingstorage>();
                var collection = db.GetCollection<UserMessage>("messages");
                collection.InsertOne(new UserMessage()
                {
                    Message = message,
                    X = x,
                    Y = y
                });

                isSuccess = true;
            }
            catch (Exception e)
            {
                //Debug.LogError(e.Message);
            }

            return isSuccess;
        }

        [ClientCallable]
        public async Promise<List<string>> GetMessage(int x, int y)
        {
            var db = await Storage.GetDatabase<testingstorage>();
            var collection = db.GetCollection<UserMessage>("messages");
            var messages = collection
               .Find(document => document.X == x && document.Y == y)
               .ToList();

            return messages.Select(m => m.Message).ToList();
        }

        [ClientCallable]
        public async Task<employee> SaveEmployee(string firstname, string lastname)
        {
            try
            {
                var db = await Storage.GetDatabase<testingstorage>();
                var collection = db.GetCollection<employee>("employees");
                var newEmployee = new employee()
                {
                    FirstName = firstname,
                    LastName = lastname
                };
                collection.InsertOne(newEmployee);
                return newEmployee;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [ClientCallable]
        public async Task<employee> GetEmployee(string id)
        {
            var db = await Storage.GetDatabase<testingstorage>();
            var collection = db.GetCollection<employee>("employees");
            if (ObjectId.TryParse(id, out var objectId))
            {

                var employee = await collection
                    .Find(document => document.Id == objectId)
                    .FirstOrDefaultAsync();

                return employee;
            }
            else
            {
                // Handle invalid ObjectId format    
                return null;
            }
        }
        [ClientCallable]
        public async Task<employee> EditEmployee(string id, string firstname, string lastname)
        {
            var db = await Storage.GetDatabase<testingstorage>();
            var collection = db.GetCollection<employee>("employees");
            if (ObjectId.TryParse(id, out var objectId))
            {
                var existingEmployee = await collection
                    .Find(document => document.Id == objectId)
                    .FirstOrDefaultAsync();

                if (existingEmployee != null)
                {

                    existingEmployee.FirstName = firstname;
                    existingEmployee.LastName = lastname;

                    var filter = Builders<employee>.Filter.Eq(e => e.Id, objectId);
                    var update = Builders<employee>.Update
                        .Set(e => e.FirstName, firstname)
                        .Set(e => e.LastName, lastname);

                    await collection.UpdateOneAsync(filter, update);

                    return existingEmployee;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        [ClientCallable]
        public async Task<string> RemoveEmployee(string id)
        {
            var db = await Storage.GetDatabase<testingstorage>();
            var collection = db.GetCollection<employee>("employees");
            if (ObjectId.TryParse(id, out var objectId))
            {
                var employee = await collection
            .Find(document => document.Id == objectId)
            .FirstOrDefaultAsync();
                var result = await collection.DeleteOneAsync(document => document.Id == objectId);

              if(result.DeletedCount > 0)
                {
                    return $"Employee {employee.FirstName} {employee.LastName} removed successfully.";
                }
                else
                {
                    return $"Employee with ID {id} not found.";
                }
            }
            else
            {
                return "Invalid ObjectId format.";
            }

        }
    }
}
