using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flurfunk.Data
{
    public abstract class Validatable
    {
        public ICollection<ValidationResult> GetValidationResults()
        {
            var context = new ValidationContext(this, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(
                this, context, results,
                validateAllProperties: true
            );
            return results;                        
        }

        public void Validate()
        {
            var context = new ValidationContext(this, serviceProvider: null, items: null);
            Validator.ValidateObject(this, context,true);
        }

    }
}
