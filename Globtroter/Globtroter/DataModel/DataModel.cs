using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Globtroter.DataModel
{
    public class Groups
    {
        public Groups() { }

        public Groups(string Name, string Id) { this.Name = Name; this.Id = Id; }

        public Groups(string Name, string Description, DateTime AddDate, string Id)
        {
            this.Name = Name;
            this.Description = Description;
            this.AddDate = AddDate;
            this.Id = Id; //Date.ToString();
        }

        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime AddDate { get; set; }
        public string Id { get; set; } //{ this.id = this.AddDate.ToString(); } }

        // Override the ToString method.
        public override string ToString()
        {
            return Name + " by ";
        }
    }

    public class Subgroups
    {
        public Subgroups() { }

        public Subgroups(string Name, string Description, DateTime AddDate, string Id, string Group)
        {
            this.Name = Name;
            this.Description = Description;
            this.AddDate = AddDate;
            this.Group = Group;
            this.Id = Id; //AddDate.ToString();
        }

        public string Group { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime AddDate { get; set; }
        public string Id { get; set; }// { this.id = this.AddDate.ToString(); } }
    }

    public class Fotos
    {
        public Fotos() { }

        public Fotos(string Name, string Description, DateTime AddDate, string Id, string Subgroup, string Lozalization)
        {
            this.Name = Name;
            this.Description = Description;
            this.AddDate = AddDate;
            this.Subgroup = Subgroup;
            this.Lozalization = Lozalization;
            this.Id = Id; //AddDate.ToString();
        }

        public string Subgroup { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime AddDate { get; set; }
        public string Lozalization { get; set; }
        public string Id { get; set; } // { this.id = this.AddDate.ToString(); } }
    }

    public class CurrentFoto
    {
        public CurrentFoto() { }
  
        public string Subgroup { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime AddDate { get; set; }
        public string Localization { get; set; }
        public string Id { get; set; } // { this.id = this.AddDate.ToString(); } }
        public WriteableBitmap _currentFoto { get; set; }
    }

    public class SubgroupsOfGroup
    {
        public SubgroupsOfGroup(string GroupName) { this.GroupName = GroupName; }

        public string GroupName { get; set; }
        public List<Subgroups> Subgroups { get; set; }
    }

}
