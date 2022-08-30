namespace Repositories.Entities
{
    public partial class ProfessorEntity
    {
        public ProfessorEntity()
        {
            Groups = new HashSet<GroupEntity>();
        }

        public string Id { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Subject { get; set; }

        public virtual ICollection<GroupEntity> Groups { get; set; }
    }
}
