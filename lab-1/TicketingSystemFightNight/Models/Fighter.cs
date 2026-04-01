using System;

namespace TicketingSystemFightNight.Models
{
    public class Fighter
    {
        public string Name { get; set; }
        public WeightClass WeightClass { get; set; }
        public FightOrganization Organization { get; set; }

        public Fighter(string name, WeightClass weightClass, FightOrganization organization)
        {
            Name = name;
            WeightClass = weightClass;
            Organization = organization;
        }
    }
}