using System.Collections.Generic;

namespace Models.DTOs
{
    public class CompanyDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }

        public virtual IList<ProductDTO> Products { get; set; }
    }
}
