namespace Framework.Domain
{
    public interface IEntity
    {
        int Id { get; set; }
        void SetId(int id);
    }
}
