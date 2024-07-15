namespace AppointmentData.Models
{
    public class Customer
    {
       
        public int CustomerId { get; set; }


        public string CustomerFullName { get; set; }
        public string CustomerPhone { get; set; }
	   
	    public string CustomerAddress { get; set; }
	    public string CustomerEmail { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int Status { get; set; }


    }
}
