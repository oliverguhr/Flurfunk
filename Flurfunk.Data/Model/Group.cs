using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flurfunk.Data.Model
{
    public class Group : Validatable
    {
        /// <summary>
        /// group Id
        /// </summary>
        public ObjectId _id { get; set; }

        /// <summary>
        /// Group Name
        /// </summary>
        [Required]
        [StringLength(40, ErrorMessage = "Must be between 3 and 40 characters", MinimumLength = 3)]
        public string Name { get; set; }
    }
}
