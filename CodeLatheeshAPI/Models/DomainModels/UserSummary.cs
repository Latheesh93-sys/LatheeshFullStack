using CodeLatheeshAPI.Models.DTO;

namespace CodeLatheeshAPI.Models.DomainModels
{
    public class UserSummary
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense{ get; set; }
        public decimal TotalInvestment { get; set; }

        public List<CategoryDto> TopExpenses { get; set; }
    }
}
