using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversityWebApplication.App_Code
{
    [MetadataType(typeof(DepartmentMetadata))]
    public partial class Department
    {

    }

    public class DepartmentMetadata
    {
        [StartDate("{0:MM-dd-yyyy}",
    ErrorMessage = "{0} value does not match the mask {1}.")]
        public object StartDate;
    }
}