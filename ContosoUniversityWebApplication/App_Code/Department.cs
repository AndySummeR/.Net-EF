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
       
        public object StartDate;
    }
}