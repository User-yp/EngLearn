namespace IdentityService.WebAPI.Controllers
{
    public record Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public Person(string Name,string Phone,string Address)
        {
            Id=Guid.NewGuid();
            this.Name  = Name;
            this.Phone = Phone;
            this.Address = Address;
        }
    }
}