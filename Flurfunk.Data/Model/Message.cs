using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Flurfunk.Data.Model
{
    [DataContract]
    public class Message : Validatable
    {
        /// <summary>
        /// message Id
        /// </summary>
        public ObjectId _id { get; set; }

        /// <summary>
        /// the message text
        /// </summary>
        //[StringLength(160,ErrorMessage = "Must be between 1 and 160 characters", MinimumLength = 1)]
        public string Text { get; set; }

        /// <summary>
        /// User who wrote this message
        /// </summary>
        //[Required]
        //[DataMember(IsRequired = true)]
        public ObjectId Creator { get; set; }

        /// <summary>
        /// If the is send to a group, this field contains the id of the group
        /// </summary>
        public ObjectId Group { get; set; }

        /// <summary>
        /// DateTime when the message was created
        /// </summary>
        //[Required]
        //[DataMember(IsRequired = true)]
        public DateTime Created { get; set; }
    }
}
