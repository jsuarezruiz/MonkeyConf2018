using System;
namespace my_clean_way.domain
{
    public class Transaction<T>
    {
        public TransactionStatus Status { get; set; }
        public T Result { get; set; }
        public String ErrorMessage { get; set; }
        public Boolean IsSuccesfull { get => TransactionStatus.Success.Equals(Status); }
    }
}
