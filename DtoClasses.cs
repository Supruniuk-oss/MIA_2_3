using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MIA_2._3
{
    public class UserDto
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
        [JsonPropertyName("username")] public string Username { get; set; } = string.Empty;
        [JsonPropertyName("email")] public string Email { get; set; } = string.Empty;
    }

    public class PostDto
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("userId")] public int UserId { get; set; }
        [JsonPropertyName("title")] public string Title { get; set; } = string.Empty;
        [JsonPropertyName("body")] public string Body { get; set; } = string.Empty;
    }
}
