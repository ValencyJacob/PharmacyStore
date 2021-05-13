using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyStore.UI.Models.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public string Summary { get; set; }

        public int? CompanyId { get; set; }
        public virtual CompanyViewModel Company { get; set; }
    }
}
