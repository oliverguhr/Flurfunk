using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Flurfunk.Data.Model
{
    public class Message : Validatable
    {
        /// <summary>
        /// message Id
        /// </summary>        
         [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        /// <summary>
        /// the message text
        /// </summary>
        [StringLength(160,ErrorMessage = "Must be between 1 and 160 characters", MinimumLength = 1)]
        public string Text { get; set; }

        /// <summary>
        /// User who wrote this message
        /// </summary>
        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CreatorId { get; set; }
        
        [BsonIgnore]
        public User Creator { get; set; }
        
        /// <summary>
        /// If the is send to a group, this field contains the id of the group
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]        
        public string Group { get; set; }

        /// <summary>
        /// DateTime when the message was created
        /// </summary>
        [Required]
        public DateTime Created { get; set; }
    }
}
