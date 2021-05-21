using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmacyStore.UI.Models.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }

        public string Image { get; set; }

        [Required]
        public string Summary { get; set; }

        [Required]
        public int CompanyId { get; set; }
        public virtual CompanyViewModel Company { get; set; }
    }
}
