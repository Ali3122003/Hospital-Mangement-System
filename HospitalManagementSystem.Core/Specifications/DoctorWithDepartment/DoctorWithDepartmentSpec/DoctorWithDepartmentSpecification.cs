using HospitalManagementSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Specifications.DoctorWithDepartment.DoctorWithDepartmentSpec
{
    public class DoctorWithDepartmentSpecification : BaseSpecifications<Doctor>
    {
        public DoctorWithDepartmentSpecification(int DeptId)
            : base(x => x.DepartmentId == DeptId)
        {
             Includes.Add(x => x.Department);
        }
    }
}
