namespace TMWebRole
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Models;
    using System.Diagnostics;

    public partial class TaskManagerContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Chore> Chores { get; set; }

        public TaskManagerContext()
           : base("name=TaskManagerContext")
           // : base("TaskManagerContext")
        {
            Database.Log = s => Debug.WriteLine(s);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
