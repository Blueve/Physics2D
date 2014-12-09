using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Physics2D.Collision;

namespace Physics2D.Core
{
    internal class ParticleContactRegistry
    {
        private List<ParticleContactGenerator> registrations = new List<ParticleContactGenerator>();
 
        public void Add(ParticleContactGenerator contactGenerator)
        {
            if (!registrations.Contains(contactGenerator))
                registrations.Add(contactGenerator);
        }

        public void Remove(ParticleContactGenerator contactGenerator)
        {
            if (registrations.Contains(contactGenerator))
                registrations.Remove(contactGenerator);
        }

        public void GenerateContactList(List<ParticleContact> contactList)
        {
            
        }
    }
}
