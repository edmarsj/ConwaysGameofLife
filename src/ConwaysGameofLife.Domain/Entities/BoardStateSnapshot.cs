using Amazon.DynamoDBv2.DataModel;

namespace ConwaysGameofLife.Domain.Entities
{
    [DynamoDBTable(nameof(BoardStateSnapshot))]
    public class BoardStateSnapshot : EntityBase
    {
        [DynamoDBHashKey("id")]
        public string Id { get; set; }
        [DynamoDBProperty("name")]
        public string Name { get; set; }
        [DynamoDBProperty("snapshot")]
        public string Snapshot { get; set; }        
    }
}
