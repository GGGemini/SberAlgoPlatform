using System.CodeDom.Compiler;
using Test4.Enums;

namespace Test4.Entities
{
    [GeneratedCode("old_tool_we_can't_modify", "1.0.0")]
    public class Employee
    {
        public string Name;
        public int Age;
        public decimal Salary;
        public MarriageStatusEnum MarriageStatus;
    }
}
