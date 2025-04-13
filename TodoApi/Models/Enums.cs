using System.Text.Json.Serialization;

namespace TodoApi.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TodoListStatus
    {
        Created,      
        InProgress,   
        Completed    
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TodoItemStatus
    {
        Created,      
        InProgress,   
        Completed     
    }
}
