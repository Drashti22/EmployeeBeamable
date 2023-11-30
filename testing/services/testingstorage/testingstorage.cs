using Beamable.Server;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Beamable.Server
{
	/// <summary>
	/// This class represents the existence of the testingstorage database.
	/// Use it for type safe access to the database.
	/// <code>
	/// var db = await Storage.GetDatabase&lt;testingstorage&gt;();
	/// </code>
	/// </summary>
	[StorageObject("testingstorage")]
	public class testingstorage : MongoStorageObject
	{
		
	}
    public class UserMessage
    {
        public ObjectId Id;
        public string Message;
        public int X;
        public int Y;
    }
	public class employee
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId Id;

		public string FirstName;
		public string LastName;
    }
}
