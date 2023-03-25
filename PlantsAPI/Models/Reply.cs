﻿using System.Text.Json.Serialization;

namespace PlantsAPI.Models
{
    public class Reply
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime DateOfCreation { get; set; }


        public Guid PostId { get; set; }
        public Guid UserId { get; set; }

        [JsonIgnore]
        public virtual Post? Post { get; set; }

        [JsonIgnore]
        public virtual User? User { get; set; }
    }
}
