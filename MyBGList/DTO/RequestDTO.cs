using MyBGList.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBGList.DTO
{
    /* The RequestDTO Class handles all the Get request HTTP calls and so, for every Get request 
     * we should be able to implement Pagination, Sort a table Column, Order Result in ascending or descending order
     * and also filder results. For the case of of SortByColumn it is ideal to be able to iterate dynamically throught various attributes 
     * or properties in our tables base on the value passed on the client. The IValidatableObject enables us to pass which Entity or Class
     * we want to iterate and get a specific field
     */
    public class RequestDTO<T> : IValidatableObject
    {
        [DefaultValue(0)]
        public int PageNumber { get; set; } = 0;

        [DefaultValue(10)]
        [Range(1, 100)]
        public int PageSize { get; set; } = 10;

        [DefaultValue("Name")]
        [SortColumnValidator(typeof(BoardGameDTO))]
        public string? SortColumn { get; set; } = "Name";

        [DefaultValue("ASC")]
        [SortOrderValidator]
        public string? SortOrder { get; set; } = "ASC";

        [DefaultValue(null)]
        public string? FilterQuery { get; set; } = null;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new SortColumnValidatorAttribute(typeof(T));
            var result = validator.GetValidationResult(SortColumn, validationContext);

            return (result != null) ? new[] { result } : new ValidationResult[0];
        }
    }
}
