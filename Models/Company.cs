using System.Collections.Generic;

namespace Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }

        public virtual IList<Product> Products { get; set; }
    }
}
