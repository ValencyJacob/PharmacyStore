using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PharmacyStore.UI.Models.ViewModels
{
    public class CompanyViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual IList<ProductViewModel> Products { get; set; }
    }
}
